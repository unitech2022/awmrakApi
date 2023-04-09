using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using awamrakeApi.Data;
using awamrakeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace carsaApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AwamrakeApiContext _context;

        public HomeController(AwamrakeApiContext context)
        {
            this._context = context;
        }

        [HttpGet("dashboard-home")]
        public async Task<ActionResult> GetDashboard()
        {

            var allSliders = await _context.Products.Where(p => p.IsSlider).ToListAsync();
            var allCategories = await _context.Categories.ToListAsync();
             var allCares = await _context.Cares.ToListAsync();
             var sittings = await _context.Sittings.ToListAsync();



            Home response = new Home
            {


                categories = allCategories,
                sliders = allSliders
       ,cares =allCares,
       sittings=sittings

            };
            return Ok(response);
        }




        [HttpGet("dashboard-home-admin")]
        public async Task<ActionResult> GetDashboardAdmin()
        {

            // List<Order> orders = new List<Order>();
            List<ResponseOrderAdmin> responseOrders = new List<ResponseOrderAdmin>();
            int countProduct = _context.Products.Count();
            int countMarkets = _context.Fields.Count();
            int countOrders = _context.Orders.Count();
            int countUsers = _context.Users.Count();






            if (countOrders > 10)
            {
                //   var  orders =  _context.Orders.TakeLast(10).ToList();

                var data = await _context.Orders.ToListAsync();
                var newData=data.TakeLast(10).ToList();
                // List<Cart> carts=new List<Cart>(); 
                foreach (var item in newData)
                {
                     Field field = _context.Fields.FirstOrDefault(p => p.Id == item.SellerId);

                   User user = await _context.Users.FindAsync(item.userId);
                responseOrders.Add(
                    new ResponseOrderAdmin
                    {
                        UserName=user.FullName,
                        UserPhone=user.UserName,
                        UserEmail=user.Email,
                        Field = field,
                        Order = item
                    }
                );
                }

            }
            else
            {
                var data = await _context.Orders.ToListAsync();
                foreach (var item in data)
                {
                    Field field = _context.Fields.FirstOrDefault(p => p.Id == item.SellerId);

                   User user = await _context.Users.FindAsync(item.userId);
                responseOrders.Add(
                    new ResponseOrderAdmin
                    {
                        UserName=user.FullName,
                        UserPhone=user.UserName,
                        UserEmail=user.Email,
                        Field = field,
                        Order = item
                    }
                );
                }
            }




            var response = new
            {
                countProduct = countProduct,
                countMarkets = countMarkets,
                countOrders = countOrders,
                countUsers = countUsers,
                orders = responseOrders
            };
            return Ok(response);
        }

    }
}
