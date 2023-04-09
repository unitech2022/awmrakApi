using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using awamrakeApi.Data;
using awamrakeApi.Dto;
using awamrakeApi.Helpers;
using awamrakeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace carsaApi.Controllers
{


    [Route("fav")]
    [ApiController]

    public class FavoriteController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AwamrakeApiContext _context;
        
        private IMapper _mapper;
        public FavoriteController( IMapper mapper, AwamrakeApiContext context, IHttpContextAccessor httpContextAccessor)
        {

            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
          

        }


        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("get-Favorites")]
        public async Task<ActionResult> GetAll()
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            var data = await _context.Favorites.Where(x => x.UserId == user.Id).ToListAsync();

            return Ok(data);
        }


        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("add-favorite")]
        public async Task<ActionResult<Favorite>> CreateFavoriteAsync([FromForm] int productId)
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);


            Favorite favorite1 = _context.Favorites.FirstOrDefault(p => p.ProductId == productId);

            if (favorite1 == null)
            {

                Product product = _context.Products.FirstOrDefault(p => p.Id == productId);
                if (product == null)
                {

                    return NotFound();
                }

                Favorite favorite = new Favorite
                {
                    BrandId = product.OfferId,
                    CategoryId = product.CategoryId,
                    Detail = product.Detail,
                    Image = product.Image,
                    Name = product.Name,
                    Price = product.Price,
                    
                    ProductId = productId,
                    SellerId = product.SellerId,
                    Status = product.Status,
                    UserId = user.Id,

                };
                
               
                await _context.Favorites.AddAsync(favorite);

                await _context.SaveChangesAsync();
                // var commandReadDto = _mapper.Map<FavoriteReadDto>(coomansModel);

                ResponseFav response = new ResponseFav
                {
                    status=true,
                    Message = "تمت الاضافة بنجاح",

                    Favorite = favorite


                };

                return Ok(response);

            }

            else
            {

                Product product = _context.Products.FirstOrDefault(p => p.Id == favorite1.ProductId);
               

               
                _context.Add(favorite1);
                await _context.SaveChangesAsync();

                ResponseFav response = new ResponseFav
                {
                    status=false,
                    Message = "تمت الحذف بنجاح",

                    Favorite = favorite1


                };

                return Ok(response);

            }



        }


        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("delete-Favorite")]
        public async Task<ActionResult> DeleteFavorite([FromForm] int id)
        {

            var FavoriteModelFromRepo = _context.Favorites.FirstOrDefault(p =>p.Id==id);
            if (FavoriteModelFromRepo == null)
            {
                return NotFound();
            }
            Product product = _context.Products.FirstOrDefault(p => p.Id == FavoriteModelFromRepo.ProductId);
           

            await _context.SaveChangesAsync();
            _context.Remove(FavoriteModelFromRepo);
            _context.SaveChanges();
            return Ok(FavoriteModelFromRepo.Name + "تم حذف");



        }



        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("update-Favorite")]
        public ActionResult UpdateFavorite([FromForm] int id, [FromForm] CreateFavoriteDto Favorite)
        {
            var favoriteModelFromRepo =  _context.Favorites.FirstOrDefault(p =>p.Id==id);
            if (favoriteModelFromRepo == null)
            {
                return NotFound();
            }

            Product product = _context.Products.FirstOrDefault(p => p.Id == favoriteModelFromRepo.ProductId);


            _mapper.Map(Favorite, favoriteModelFromRepo);

           

            _context.SaveChanges();

            return NoContent();

        }

    }
}