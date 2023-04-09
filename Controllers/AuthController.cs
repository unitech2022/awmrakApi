using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

using awamrakeApi.Data;
using awamrakeApi.Dto;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using awamrakeApi.Models;

namespace awamrakeApi.Controllers{





    public class AuthController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private UserManager<User> userManager;
        private readonly IConfiguration _config;
        private readonly AwamrakeApiContext _context;
        public readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(IWebHostEnvironment webHostEnvironment, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, AwamrakeApiContext context
                     )
        {
            this._roleManager = roleManager;
            this._mapper = mapper;
            this.userManager = userManager;
            this._config = config;
            this._webHostEnvironment = webHostEnvironment;
            this._context = context;
        }


        [HttpPost("auth/check-username")]
        public async Task<Object> checkPhone([FromForm] string phone)
        {
            User user = await _context.Users.Where(x => x.UserName == phone).FirstOrDefaultAsync();
            string Code = "";
            if (user != null)
            {
                if (user.Code == null)
                {
                    Code = RandomNumber();
                    user.Code = Code;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    Code = user.Code;
                }
                return Ok(new
                {
                    status = 1,
                    Code = Code,
                });
            }
            else
            {
                Code = RandomNumber();

                return Ok(new
                {
                    status = 0,
                    Code = Code,
                });
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [NonAction]
        public string RandomNumber()
        {
            Random r = new Random();
            int randNum = r.Next(1000);
            string fourDigitNumber = randNum.ToString("D4");
            return fourDigitNumber;
        }

        [HttpGet("test")]
        public ActionResult Test([FromForm] UserForRegister model)
        {
            return View("index");
        }



        [HttpPost("auth/signup")]
        public async Task<ActionResult> register([FromForm] UserForRegister model)
        {
            model.Password = "Abc123@";
            var userToCreate = _mapper.Map<User>(model);
            userToCreate.Role = model.Role;
            userToCreate.DeviceToken=model.DeviceFCM;
            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            var result = await userManager.CreateAsync(userToCreate, model.Password);
            await userManager.AddToRoleAsync(userToCreate, model.Role);
            string Code = RandomNumber();
            userToCreate.Code = Code;
            await _context.SaveChangesAsync();
            return Ok(new { success = true, Code = Code });
        }

        [HttpPost("auth/admin/signup")]
        public async Task<ActionResult> RegisterAdmin(UserForRegister model)
        {
            var userToCreate = _mapper.Map<User>(model);
            userToCreate.Role = model.Role;
            if (!await _roleManager.RoleExistsAsync(model.Role))
                await _roleManager.CreateAsync(new IdentityRole(model.Role));
            var result = await userManager.CreateAsync(userToCreate, model.Password);
            await userManager.AddToRoleAsync(userToCreate, model.Role);

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }



        [HttpPost("auth/driver-signup")]
        public async Task<ActionResult> registerDriver(DriverForRegister model)
        {
            model.Password = "Abc123@";
            var userToCreate = _mapper.Map<User>(model);
            userToCreate.Role = "driver";
            if (!await _roleManager.RoleExistsAsync("driver"))
                await _roleManager.CreateAsync(new IdentityRole("driver"));
            await userManager.CreateAsync(userToCreate, model.Password);
            await userManager.AddToRoleAsync(userToCreate, "driver");

            Driver driver = _mapper.Map<Driver>(model);

            driver.UserId = userToCreate.Id;
            await _context.Drivers.AddAsync(driver);
            await _context.SaveChangesAsync();
            return Ok(model);
        }





        [HttpPost("auth/validate")]
        public async Task<ActionResult> validate([FromForm] UserForValidate userForValidate)
        {
            string error = "";
            User user = await _context.Users.Where(x => x.UserName == userForValidate.UserName).FirstOrDefaultAsync();
            if (user != null)
            {
                error = "رقم الهاتف مسجل من قبل";
                return this.StatusCode(StatusCodes.Status200OK, error);
            }
            user = await _context.Users.Where(x => x.Email == userForValidate.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                error = "البريد الإلكتروني مسجل من قبل";
                return BadRequest(error);
            }

            return Ok("success");
        }






        [HttpPost("/auth/admin-login")]
        public async Task<IActionResult> LoginAdmin(AdminForLoginRequest adminForLogin)
        {
            var loginUser = await userManager.FindByNameAsync(adminForLogin.UserName);
            if (loginUser != null && await userManager.CheckPasswordAsync(loginUser, adminForLogin.Password))
            {
                var userRoles = await userManager.GetRolesAsync(loginUser);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loginUser.Id),
                    new Claim(ClaimTypes.Name, loginUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _config["JWT:ValidIssuer"],
                    audience: _config["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(100),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                UserDetailResponse user = _mapper.Map<UserDetailResponse>(loginUser);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    user,

                    userRoles,
                    expiration = token.ValidTo,

                });
            }
            return Unauthorized();
        }





        [HttpPost("/auth/confirm-Code")]
        public async Task<IActionResult> Login([FromForm] UserForLogin model)                     
        {
            var loginUser = await userManager.FindByNameAsync(model.userName);
            var driver = await _context.Drivers.Where(x => x.UserId == loginUser.Id).FirstOrDefaultAsync();
            if (loginUser != null)
            {
                var userRoles = await userManager.GetRolesAsync(loginUser);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loginUser.Id),
                    new Claim(ClaimTypes.Name, loginUser.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _config["JWT:ValidIssuer"],
                    audience: _config["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(100),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                UserDetailResponse user = _mapper.Map<UserDetailResponse>(loginUser);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    user,
                    driver = driver,
                    userRoles,
                    expiration = token.ValidTo,
                });
            }
            return Unauthorized();
        }

        // [Authorize]
        [HttpPost("auth/update-deviceToken")]
        public async Task<ActionResult> updateToken([FromForm] string Token, [FromForm] string UserId)
        {
            User user = await _context.Users.Where(x => x.Id == UserId).FirstAsync();
            user.DeviceToken = Token;
            await _context.SaveChangesAsync();
            return Ok("Updated Successfully");

        }


        

        //login provider



          
        

    }
}