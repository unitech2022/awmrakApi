using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using awamrakeApi.Data;
using awamrakeApi.Helpers;
using awamrakeApi.Models;
using awamrakeApi.Dto;
using awamrakeApi.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace carsaApi.Controllers
{

    [Route("order")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AwamrakeApiContext _context;


        private IMapper _mapper;
        public OrdersController(IMapper mapper, AwamrakeApiContext context, IHttpContextAccessor httpContextAccessor)
        {

            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }


        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("get-Orders")]
        public async Task<ActionResult> GetAll()
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            var data = await _context.Orders.Where(x => (x.userId == user.Id && x.Status != -1)).ToListAsync();

            return Ok(data);
        }



        [HttpGet]
        [Route("get-all-Orders")]
        public async Task<ActionResult> GetAllOrders()
        {


            List<ResponseOrder> responseOrders = new List<ResponseOrder>();

            var data = await _context.Orders.ToListAsync();
            // List<Cart> carts=new List<Cart>();
            foreach (var item in data)
            {
                User user = await _context.Users.FindAsync(item.userId);
                Address address = _context.Addresses.FirstOrDefault(p => p.Id == item.AddressId);

                var carts = _context.Carts.Where(p => p.OrderId == item.Id).ToList();


                responseOrders.Add(new ResponseOrder
                {
                    Order = item,
                    UserEmail = user.Email,
                    UserName = user.FullName,
                    UserPhone = user.UserName,
                    Address = address,
                    Products = carts,
                });

            }

            return Ok(responseOrders);
        }




        // order Market
        [Authorize(Roles = "provider")]
        [HttpGet]
        [Route("get-market-Orders")]
        public async Task<ActionResult> GetMarketOrders([FromForm] int market_id)
        {


            List<ResponseOrder> responseOrders = new List<ResponseOrder>();

            var data = await _context.Orders.Where(p => p.SellerId == market_id && p.Status !=0).ToListAsync();
            // List<Cart> carts=new List<Cart>();
            foreach (var item in data)
            {
                User user = await _context.Users.FindAsync(item.userId);
                Address address = _context.Addresses.FirstOrDefault(p => p.Id == item.AddressId);

                var carts = _context.Carts.Where(p => p.OrderId == item.Id).ToList();


                responseOrders.Add(new ResponseOrder
                {
                    Order = item,
                    UserEmail = user.Email,
                    UserName = user.FullName,
                    UserPhone = user.UserName,
                    Address = address,
                    Products = carts,
                });

            }

            return Ok(responseOrders);
        }


      
        [HttpGet]
        [Route("get-Order-details=admin")]
        public async Task<ActionResult> GetOrderDetailsAdmin([FromQuery] int orderId)
        {

          
            var data = await _context.Carts.Where(x => x.OrderId == orderId).ToListAsync();


            return Ok(data);

        }



        [Authorize(Roles = "user")]
        [HttpGet]
        [Route("get-Order-details")]
        public async Task<ActionResult> GetOrderDetails([FromForm] int orderId)
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            var data = await _context.Carts.Where(x => (x.UserId == user.Id && x.OrderId == orderId)).ToListAsync();

            return Ok(data);

        }

        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("add-order")]
        public async Task<ActionResult> CreateOrder([FromForm] Order order)
        {
            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            order.userId = user.Id;

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            var data = await _context.Carts.Where(x => (x.UserId == user.Id && x.OrderId == 0)).ToListAsync();
            foreach (var cart in data)
            {

                cart.OrderId = order.Id;
                order.SellerId = cart.market_id;
                await _context.SaveChangesAsync();

            }
            // Functions.SendNotificationFromFirebaseCloud(
            //     new NotificationData
            //     {
            //         Body = "تم نشر اعلان في المعلقة",
            //         Desc = "fffffffffff",
            //         ImageUrl = "dddd",
            //         Title = "ddddddddddddd",
            //         Subject = "mdmdmddmd"
            //     }

            // );

            return Ok(order);


        }


        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("delete-Order")]
        public async Task<ActionResult> DeleteOrder([FromForm] int id)
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);

            Order order = _context.Orders.FirstOrDefault(p => p.Id == id && p.userId == user.Id);

            if (order == null)
            {
                return NotFound();
            }


            _context.Remove(order);
            await _context.SaveChangesAsync();
            var data = await _context.Carts.Where(x => (x.UserId == user.Id && x.OrderId == id)).ToListAsync();

            foreach (var cart in data)
            {

                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }







            return Ok(order);



        }




        //  [Authorize(Roles = "user")]
        [HttpPost]
        [Route("delete-Order-admin")]
        public async Task<ActionResult> DeleteOrderAdmin([FromForm] int id)
        {

            // User user = await Functions.getCurrentUser(_httpContextAccessor, _context);

            Order order = _context.Orders.FirstOrDefault(p => p.Id == id);

            if (order == null)
            {
                return NotFound();
            }


            _context.Remove(order);
            await _context.SaveChangesAsync();
            return Ok(order);

        }

        [Authorize(Roles = "user")]
        [HttpPost]
        [Route("update-Order")]
        public async Task<ActionResult> UpdateCategory([FromForm] int id, [FromForm] OrderCreateDto Order)
        {

            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);

            var OrderModelFromRepo = _context.Orders.FirstOrDefault(p => p.Id == id && p.userId == user.Id);
            if (OrderModelFromRepo == null)
            {
                return NotFound();
            }


            _mapper.Map(Order, OrderModelFromRepo);

            _context.SaveChanges();

            // _repository.SaveChanges();

            return NoContent();

        }

        [Authorize(Roles = "provider")]
        [HttpPost]
        [Route("update-status-Order")]
        public async Task<ActionResult> UpdateOrderStatus([FromForm] int id, [FromForm] int status, [FromForm] int market_id)
        {
            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            var orderModelFromRepo = _context.Orders.FirstOrDefault(p => p.Id == id && p.SellerId == market_id);
            if (orderModelFromRepo == null)
            {
                return NotFound();
            }

            orderModelFromRepo.Status = status;
            _context.SaveChanges();


            return Ok(orderModelFromRepo);

        }




        [Authorize(Roles = "provider")]
        [HttpGet]
        [Route("get-by-market")]
        public async Task<ActionResult> GetOrderByMarket([FromForm] int market_id)
        {


            User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            var data = await _context.Orders.Where(x => (x.SellerId == market_id)).ToListAsync();


            return Ok(data);
        }

        // admin

        // [Authorize(Roles = "user")]
        [HttpGet]
        [Route("get-Orders-admin")]
        public async Task<ActionResult> GetAllAdmin()
        {

            // User user = await Functions.getCurrentUser(_httpContextAccessor, _context);
            List<ResponseOrderAdmin> responseOrders = new List<ResponseOrderAdmin>();
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



            return Ok(responseOrders);
        }


        // update
        [HttpPost("update-status-order-admin")]
        public ActionResult UpdateProductStatus([FromForm] int id, [FromForm] int status)

        {

            var product = _context.Orders.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }



            product.Status = status;


            _context.SaveChanges();

            return Ok(product);

        }
    }
}