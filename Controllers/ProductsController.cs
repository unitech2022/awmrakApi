using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using awamrakeApi.Data;
using awamrakeApi.Helpers;
using awamrakeApi.Models;
using awamrakeApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace awamrakeApi.Controllers
{

    [Route("product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {




        IHttpContextAccessor _httpContextAccessor;
        private readonly AwamrakeApiContext _context;

        private IMapper _mapper;
        public ProductsController(IMapper mapper, AwamrakeApiContext context, IHttpContextAccessor httpContextAccessor)
        {

            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }



        [HttpGet]
        [Route("get-products")]
        public async Task<ActionResult> GetAll([FromForm] string city)
        {


            List<Product> fields = new List<Product> { };

            if (city != null)
            {
                fields = await _context.Products.Where(p => p.City == city && p.Status == 1).AsNoTracking().ToListAsync();

            }
            else
            {
                fields = await _context.Products.Where(p => p.Status == 1).AsNoTracking().ToListAsync();


            }



            return Ok(fields);


        }


        [HttpGet]
        [Route("get-offer-products")]
        public async Task<ActionResult> GetAllOffers([FromForm] string city)
        {


            List<Product> fields = new List<Product> { };

            if (city != null)
            {
                fields = await _context.Products.Where(p => p.City == city && p.OfferId != 0).AsNoTracking().ToListAsync();

            }
            else
            {
                fields = await _context.Products.Where(p => p.OfferId != 0).AsNoTracking().ToListAsync();


            }



            return Ok(fields);


        }

        [HttpGet]
        [Route("search-products")]
        public async Task<ActionResult> SearchProduct([FromForm] string name, [FromForm] string city,
         [FromForm] int categoryId)
        {


            List<Product> fields = new List<Product> { };

            if (city != null)
            {

                fields = await _context.Products.Where(p => p.Name.Contains(name) && p.City == city && p.HomeCategoryId == categoryId).ToListAsync();
            }
            else
            {
                fields = await _context.Products.Where(p => p.Name.Contains(name) && p.HomeCategoryId == categoryId).ToListAsync();


            }




            return Ok(fields);
        }




        [HttpGet]
        [Route("search-markets")]
        public async Task<ActionResult> SearchMarkets([FromForm] string name, [FromForm] string city,
              [FromForm] int categoryId)
        {


            List<Field> fields = new List<Field> { };

            if (city != null)
            {

                fields = await _context.Fields.Where(p => p.name.Contains(name) && p.City == city && p.CategoryId == categoryId).ToListAsync();
            }
            else
            {
                fields = await _context.Fields.Where(p => p.name.Contains(name) && p.CategoryId == categoryId).ToListAsync();


            }




            return Ok(fields);
        }





        [HttpGet]
        [Route("get-products-by-category")]
        public async Task<ActionResult> GetAllByCategory([FromForm] int categoryId, [FromForm] string city)
        {

            List<Product> fields = new List<Product> { };

            if (city != null)
            {
                fields = await _context.Products.Where(p => p.City == city && p.CategoryId == categoryId && p.Status == 1).AsNoTracking().ToListAsync();

            }
            else
            {
                fields = await _context.Products.Where(p => p.CategoryId == categoryId && p.Status == 1).ToListAsync();


            }



            return Ok(fields);




        }



        [HttpGet]
        [Route("get-products-by-id")]
        public async Task<ActionResult> GetAllByCategoryAdmin([FromQuery] int categoryId, [FromQuery] string city)
        {

            List<Product> fields = new List<Product> { };

            if (city != null)
            {
                fields = await _context.Products.Where(p => p.City == city && p.CategoryId == categoryId).AsNoTracking().ToListAsync();

            }
            else
            {
                fields = await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();


            }



            return Ok(fields);




        }



        [HttpGet]
        [Route("get-product-details")]
        public ActionResult<Product> GetProduct([FromForm] int id)
        {

            Product product = _context.Products.FirstOrDefault(p => p.Id == id);



            return Ok(product);




        }



        [HttpPost]
        [Route("add-product")]
        public async Task<ActionResult<Product>> CreateCategory([FromForm] Product product)
        {


            // var commandReadDto = _mapper.Map<CategoryReadDto>(coomansModel);
            await _context.Products.AddAsync(product);
            _context.SaveChanges();
            return Ok(product);




        }


        [Authorize(Roles = "provider")]
        [HttpPost]
        [Route("delete-product")]
        public async Task<ActionResult> DeleteCategory([FromForm] int id)
        {
            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            Product categoryModelFromRepo = _context.Products.FirstOrDefault(p => p.Id == id && p.SellerId == user.Id);
            if (categoryModelFromRepo == null)
            {
                return NotFound();
            }

            _context.Remove(categoryModelFromRepo);
            _context.SaveChanges();
            return Ok(categoryModelFromRepo);



        }





        [HttpPost]
        [Route("delete-product=admin")]
        public ActionResult DeleteProductAdmin([FromForm] int id)
        {

            Product categoryModelFromRepo = _context.Products.FirstOrDefault(p => p.Id == id);
            if (categoryModelFromRepo == null)
            {
                return NotFound();
            }

            _context.Remove(categoryModelFromRepo);
            _context.SaveChanges();
            return Ok(categoryModelFromRepo);



        }





        [HttpPost("{id}")]
        [Route("update-product")]
        public async Task<ActionResult> UpdateProduct([FromForm] int id, [FromForm] CreateProductDto productDto)
        {
            var categoryModelFromRepo = _context.Products.FirstOrDefault(p => p.Id == id);
            if (categoryModelFromRepo == null)
            {
                return NotFound();
            }


            _mapper.Map(productDto, categoryModelFromRepo);



            await _context.SaveChangesAsync();

            return NoContent();

        }






        // update
        [HttpPost("update-status-product")]
        public ActionResult UpdateProductStatus([FromForm] int id, [FromForm] int status)

        {

            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }



            product.Status = status;


            _context.SaveChanges();

            return Ok(product);

        }



        //=======================================================
        // products in Slider
        [HttpGet]
        [Route("get-products-sliders")]
        public async Task<ActionResult> GetAllSliders()
        {
            var data = await _context.Products.Where(p => p.IsSlider).ToListAsync();
            return Ok(data);

        }





//================= pagination products =================


    [HttpGet]
        [Route("get-products-by-id-Page-admin")]
        public async Task<ActionResult> GetAllByCategoryAdminPage([FromQuery] int categoryId,[FromQuery]PagingParameterModel  @params)
        {

          

         
        
               var data = await _context.Products.Where(p =>  p.CategoryId == categoryId).ToListAsync();




               var paginationMetadata = new PaginationMetadata(data.Count(), @params.Page, @params.ItemsPerPage);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var items =  data.Skip((@params.Page - 1) * @params.ItemsPerPage)
                                       .Take(@params.ItemsPerPage);
    return Ok(new {

        items=items,
        currentPage=@params.Page,
        totalPage=paginationMetadata.TotalPages
    });




        }






                [HttpGet]
        [Route("get-products-by-category-user")]
        public async Task<ActionResult> GetAllByCategory([FromForm] int categoryId,[FromForm]PagingParameterModel  @params)
        {

            List<Product> fields = new List<Product> { };

          
                fields = await _context.Products.Where(p =>  p.CategoryId == categoryId && p.Status == 1).ToListAsync();

                    var paginationMetadata = new PaginationMetadata(fields.Count(), @params.Page, @params.ItemsPerPage);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var items =  fields.Skip((@params.Page - 1) * @params.ItemsPerPage)
                                       .Take(@params.ItemsPerPage);
    return Ok(new {

        items=items,
        currentPage=@params.Page,
        totalPage=paginationMetadata.TotalPages
    });



       




        }




        // =================
        [HttpGet]
        [Route("get-offer-products-page")]
        public async Task<ActionResult> GetAllOffersPage([FromForm] string city,[FromForm]PagingParameterModel  @params)
        {


            List<Product> fields = new List<Product> { };

            if (city != null)
            {
                fields = await _context.Products.Where(p => p.City == city && p.OfferId != 0).AsNoTracking().ToListAsync();

            }
            else
            {
                fields = await _context.Products.Where(p => p.OfferId != 0).AsNoTracking().ToListAsync();


            }



         var paginationMetadata = new PaginationMetadata(fields.Count(), @params.Page, @params.ItemsPerPage);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var items =  fields.Skip((@params.Page - 1) * @params.ItemsPerPage)
                                       .Take(@params.ItemsPerPage);
          return Ok(new {

        items=items,
        currentPage=@params.Page,
        totalPage=paginationMetadata.TotalPages
        });


        }


    }
}