using AutoMapper;
using awamrakeApi.Data;
using awamrakeApi.Dto;
using awamrakeApi.Helpers;
using awamrakeApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

using System.Threading.Tasks;
using System.Text.Json;
namespace awamrakeApi.Controllers
{
    public class FieldsController : Controller
    {

        private readonly IMapper _mapper;
        private readonly AwamrakeApiContext myDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FieldsController(IMapper mapper, AwamrakeApiContext context
                    , IHttpContextAccessor httpContextAccessor)
        {
            this._mapper = mapper;
            this.myDbContext = context;
            _httpContextAccessor = httpContextAccessor;
        }


        [Authorize(Roles = "provider")]
        [HttpPost("field/add")]
        public async Task<ActionResult> addField(Field modal)
        {
            User user = await Functions.getCurrentUser(_httpContextAccessor, myDbContext);
            Field field = myDbContext.Fields.FirstOrDefault(p => p.UserId == modal.UserId);


            if (field == null)
            {

                await myDbContext.Fields.AddAsync(modal);
                myDbContext.SaveChanges();
                return Ok(new
                {
                    status = true,
                    market = modal
                });
            }
            else
            {

                return Ok(new
                {
                    status = false,
                    market = modal
                }
                );
            }


        }



        // user page

        [HttpGet("field/get-fields")]
        public async Task<ActionResult> getFields([FromForm] string City, [FromForm] int categoryId)
        {

            List<Field> fields = new List<Field> { };

            if (City != null)
            {
                fields = await myDbContext.Fields.Where(p => p.City == City && p.CategoryId == categoryId && p.status == 1).ToListAsync();


            }
            else
            {
                fields = await myDbContext.Fields.Where(p => p.CategoryId == categoryId && p.status == 1).ToListAsync();


            }



            return Ok(fields);
        }





  [HttpGet("field/get-fields-page-user")]
        public async Task<ActionResult> getFieldsPageUser([FromForm] string City, [FromForm] int categoryId,[FromForm]PagingParameterModel  @params)
        {

            List<Field> fields = new List<Field> { };

            if (City != null)
            {
                fields = await myDbContext.Fields.Where(p => p.City == City && p.CategoryId == categoryId && p.status == 1).ToListAsync();
           
            }
            else
            {
           fields =await  myDbContext.Fields.OrderBy(p => p.Id).Where(p => p.CategoryId == categoryId && p.status == 1).ToListAsync();

  

            }


            



   var paginationMetadata = new PaginationMetadata(fields.Count(), @params.Page, @params.ItemsPerPage);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var items =  fields.Skip((@params.Page - 1) * @params.ItemsPerPage)
                                       .Take(@params.ItemsPerPage)
                                       ;
    return Ok(new {

        items=items,
        currentPage=@params.Page,
        totalPage=paginationMetadata.TotalPages
    });


           
        }



       

// Get Market

        [HttpGet("field/get-field-by-id")]
        public async Task<ActionResult> getFieldById([FromForm] int id)
        {

            var brandModelFromRepo = await myDbContext.Fields.FirstOrDefaultAsync(p => p.Id == id);
            if (brandModelFromRepo == null)
            {
                return NotFound();
            }

   return Ok(brandModelFromRepo);
           
        
        }



// pagination admin================================

           [HttpGet("field/get-fields-page-admin")]
        public async Task<IActionResult> getFieldsPage([FromQuery]PagingParameterModel  @params)
        {

            
           
               var fields =  myDbContext.Fields.OrderBy(p => p.Id);


         



          var paginationMetadata = new PaginationMetadata(fields.Count(), @params.Page, @params.ItemsPerPage);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var items = await fields.Skip((@params.Page - 1) * @params.ItemsPerPage)
                                       .Take(@params.ItemsPerPage)
                                       .ToListAsync();
    return Ok(new {

        items=items,
        currentPage=@params.Page,
        totalPage=paginationMetadata.TotalPages
    }); 



          
        }



         [HttpGet("field/get-all-fields")]
        public async Task<ActionResult> getAllFields([FromForm] string City)
        {

            List<Field> fields = new List<Field> { };

            if (City != null)
            {
                fields = await myDbContext.Fields.Where(p => p.City == City).ToListAsync();


            }
            else
            {
                fields = await myDbContext.Fields.ToListAsync();


            }



            return Ok(fields);
        }




        [HttpPost("field/remove-field")]
        public async Task<ActionResult> RemoveField([FromForm] int id)
        {

            var field = myDbContext.Fields.FirstOrDefault(p => p.Id == id);
            if (field != null)
            {

                myDbContext.Fields.Remove(field);
                await myDbContext.SaveChangesAsync();
                List<Product> products = await myDbContext.Products.Where(p => p.CategoryId == field.Id).ToListAsync();

                foreach (var data in products)
                {


                    myDbContext.Products.Remove(data);
                    await myDbContext.SaveChangesAsync();
                }




            }



            return Ok(field);
        }


        [HttpPost("field/update-field")]
        public async Task<ActionResult> UpdateField([FromForm] int id, [FromForm] CreateFieldDto field)




        {

            var brandModelFromRepo = await myDbContext.Fields.FirstOrDefaultAsync(p => p.Id == id);
            if (brandModelFromRepo == null)
            {
                return NotFound();
            }


            _mapper.Map(field, brandModelFromRepo);

            // _repository.UpdateCategory(brandModelFromRepo);

            await myDbContext.SaveChangesAsync();

            return Ok(field);

        }





        // update
        [HttpPost("field/update-status-field")]
        public async Task<ActionResult> UpdateFieldStatus([FromForm] int id, [FromForm] int status)

        {

            var brandModelFromRepo = myDbContext.Fields.FirstOrDefault(p => p.Id == id);
            if (brandModelFromRepo == null)
            {
                return NotFound();
            }



            brandModelFromRepo.status = status;


            await myDbContext.SaveChangesAsync();

            return Ok(brandModelFromRepo);

        }






        [Authorize(Roles = "provider")]
        [HttpGet("field/get-market")]
        public async Task<ActionResult> getMarket([FromForm] string City, [FromForm] String uderId)
        {
            User user = await Functions.getCurrentUser(_httpContextAccessor, myDbContext);
            Field fields = myDbContext.Fields.FirstOrDefault(p => p.UserId == uderId);


            if (fields != null)
            {



                return Ok(new
                {
                    status = true,
                    market = fields,
                });
            }
            else
            {

                return Ok(new
                {
                    status = false,
                    market = new Field(),
                });
            }


        }

        [HttpPost("market/update-deviceToken")]
        public async Task<ActionResult> updateToken([FromForm] string Token, [FromForm] int UserId)
        {
            Field user = await myDbContext.Fields.Where(x => x.Id == UserId).FirstAsync();
            user.Token = Token;
            await myDbContext.SaveChangesAsync();
            return Ok("Updated Successfully");

        }










        // [HttpGet("field/markets")]
        // public async Task<ActionResult> getMarkets(int fieldId,  int addressId)
        // {
        //     //if (addressId == 0|| addressId == null) addressId = 4;
        //     List<MarketDetailResponse> markets = new List<MarketDetailResponse>();

        //     var radiusInMile = 30;
        //     Address address = await myDbContext.addresses.Where(x => x.Id == addressId).FirstOrDefaultAsync();
        //     if (address == null) {
        //         radiusInMile = 10000000;
        //         address = new Address()
        //         {
        //             lat = 0.0,
        //             lng  = 0.0
        //         };
        //     }
        //     var myLat = address.lat;
        //     var myLon = address.lng;
        //     var fieldMarkets = myDbContext.fieldMarkets.Where(x =>fieldId!=20? x.field_id == fieldId:x.field_id>0)
        //    .AsEnumerable()
        //    .Select(fm => new { fm, 
        //     Dist = distanceInMiles(myLon, myLat, fm.lng, fm.lat)
        //    }).OrderBy(market => market.Dist)
        //    .Where(p => p.Dist <= radiusInMile);
        //     foreach (var fm in fieldMarkets)
        //     {
        //         List<Field> fields = new List<Field>();

        //         var market = await myDbContext.markets.Where(x => x.Id == fm.fm.market_id).FirstAsync();
        //         var marketFields = await myDbContext.fieldMarkets.Where(x => x.market_id == market.Id).ToArrayAsync();
        //         foreach (var mf in marketFields)
        //         {
        //             var field = await myDbContext.fields.Where(x => x.Id == mf.field_id).FirstAsync();
        //             fields.Add(field);
        //         }

        //         var fods = await myDbContext.foods.Where(x=>x.market_id==market.Id).AsNoTracking().ToListAsync();
        //         List<FoodDetailResponse> foods = new List<FoodDetailResponse>();
        //         foreach (var food in fods)
        //         {
        //             List<Photo> photos = await myDbContext.photos.Where(x => x.Modle == "food" && x.ModleId == food.Id.ToString()).ToListAsync();
        //             FoodDetailResponse foodDetail = new FoodDetailResponse()
        //             {
        //                 food = food,
        //                 photos = photos
        //             };
        //             foods.Add(foodDetail);
        //         }
        //         MarketDetailResponse marketDetail = new MarketDetailResponse
        //         {
        //             fields = fields,
        //             market = market,
        //             foods=foods,
        //             dist = fm.Dist.ToString()

        //         };
        //         markets.Add(marketDetail);
        //         }
        //     return Ok(markets);
        // }

        public double ToRadians(double degrees) => degrees * Math.PI / 180.0;
        public double distanceInMiles(double lon1d, double lat1d, double lon2d, double lat2d)
        {
            var lon1 = ToRadians(lon1d);
            var lat1 = ToRadians(lat1d);
            var lon2 = ToRadians(lon2d);
            var lat2 = ToRadians(lat2d);
            var deltaLon = lon2 - lon1;
            var c = Math.Acos(Math.Sin(lat1) * Math.Sin(lat2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Cos(deltaLon));
            var earthRadius = 3958.76;
            var distInMiles = earthRadius * c;
            return Math.Round(distInMiles, 2);
        }

    }
}
