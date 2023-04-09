using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using awamrakeApi.Data;
using awamrakeApi.Helpers;
using awamrakeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace carsaApi.Controllers
{

    [Route("cart")]
    [ApiController]
    public class CartsController : ControllerBase
    {


        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AwamrakeApiContext _context;
        private IMapper _mapper;
        public CartsController(IMapper mapper, AwamrakeApiContext context, IHttpContextAccessor httpContextAccessor)
        {

            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("get-carts")]
        public async Task<ActionResult> GetAll()
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            var data = await _context.Carts.Where(x => (x.UserId == user.Id && x.OrderId == 0)).ToListAsync();

            return Ok(data);
        }


   [Authorize(Roles = "user")]
        [HttpGet]
        [Route("get-all-carts")]
        public async Task<ActionResult> GetAllCarts()
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            var data = await _context.Carts.Where(x => (x.UserId == user.Id )).ToListAsync();

            return Ok(data);
        }




        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("add-cart")]
        public async Task<ActionResult<Cart>> CreateCategoryAsync([FromForm] Cart Cart)
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);

            var categoryModelFromRepo = _context.Carts.FirstOrDefault(x => x.ProductId == Cart.ProductId
            && x.OrderId==0);


            if (categoryModelFromRepo == null)
            {



                var carts = await _context.Carts.Where(x => x.OrderId == 0 && x.UserId == Cart.UserId).ToListAsync();

                if (carts.LastOrDefault() != null)
                {
                    Cart lastCart = carts[carts.Count - 1];
                    if (lastCart.market_id != Cart.market_id && lastCart.OrderId == 0)
                    {
                        _context.Carts.RemoveRange(carts);
                        await _context.SaveChangesAsync();
                    }
                }

                Cart.UserId = user.Id;

                Product product = _context.Products.FirstOrDefault(p => p.Id == Cart.ProductId);

                Cart.Price = product.Price;

                Cart.Total = Cart.Quantity * product.Price;

                // Cart.market_id = product.CategoryId;

                await _context.Carts.AddAsync(Cart);

                _context.SaveChanges();
                // var commandReadDto = _mapper.Map<CategoryReadDto>(coomansModel);
                return Ok(Cart);

            }
            else
            {



                _context.Carts.Remove(categoryModelFromRepo);
                _context.SaveChanges();
                return Ok(categoryModelFromRepo);


            }




        }


        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("delete-cart")]
        public async Task<ActionResult> DeleteCart([FromForm] int id)
        {


            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);


            var categoryModelFromRepo = _context.Carts.FirstOrDefault(x => (x.Id == id && x.UserId == user.Id));
            if (categoryModelFromRepo == null)
            {
                return NotFound();
            }


            _context.Carts.Remove(categoryModelFromRepo);
            _context.SaveChanges();
            return Ok(categoryModelFromRepo.NameProduct + "تم حذف");



        }



        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("update-Cart")]
        public async Task<ActionResult> UpdateCategory([FromForm] Cart Cart)
        {
            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            var CartModelFromRepo = _context.Carts.FirstOrDefault(x => x.UserId == user.Id && Cart.Id == x.Id);
            if (CartModelFromRepo == null)
            {
                return NotFound();
            }

            Product product = _context.Products.FirstOrDefault(p => p.Id == CartModelFromRepo.ProductId);

            CartModelFromRepo.Quantity = Cart.Quantity;
            CartModelFromRepo.Total = product.Price * Cart.Quantity;

            await _context.SaveChangesAsync();

            return Ok(CartModelFromRepo);

        }

    }
}