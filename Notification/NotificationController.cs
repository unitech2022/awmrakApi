using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using awamrakeApi.Models;
using awamrakeApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Newtonsoft.Json;

namespace awamrakeApi.Notification
{




    [Route("api/notification")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly AwamrakeApiContext _context;
        public NotificationController(INotificationService notificationService,AwamrakeApiContext context)
        {
            _notificationService = notificationService;
            _context=context;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendNotification([FromForm] NotificationModel notificationModel,[FromForm] string userId)
        {


               
               Notifications notey=new Notifications{

                name =notificationModel.Title,
                Body = notificationModel.Body,
                 UserId =userId,
                Image=""
               };

               await _context.Notifications.AddAsync(notey);
             await _context.SaveChangesAsync();

            var result = await _notificationService.SendNotification(notificationModel);
            // SendNotificationFromFirebaseCloud();
            return Ok(result);
        }


        // FirebaseMessaging.getInstance().subscribeToTopic("news");
        [Route("send/topic")]
        [HttpPost]
        public async Task<IActionResult> SendNotificationFromFirebaseCloud([FromForm] NotificationData data,[FromForm] string topice)
        {


              Notifications notey =new Notifications{

                name =data.Title,
                Body = data.Body,
                 UserId = topice,
                Image=""
               };

               await _context.Notifications.AddAsync(notey);

                await _context.SaveChangesAsync();



            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAA835VwYI:APA91bFSluwCVfPBIzWKHegws_ahGs9kyk2iUhmg4c7fep0hG1Kz6ofTrNsWIzh2NZNAHxUwhVujCb4nNjFL4W91Vi13qlZNTOQFobbWeBIy_J3t14qz-NUEL3N6HhqkTr0PxhJGx0E2"));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", "1045796602242"));
            tRequest.ContentType = "application/json";
            var payload = new
            {
                to = "/topics/"+topice,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = data.Body,
                    title = data.Title,
                    badge = 1
                },
                data = new
                {
                    subject = data.Subject,
                    imageUrl = data.ImageUrl,
                    desc = data.Desc,
                    data = data.createAt
                }

            };

            string postbody = JsonConvert.SerializeObject(payload).ToString();
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                //result.Response = sResponseFromServer;
                            }
                    }
                }
            }

            return Ok(data);
        }



       [HttpGet]
        [Route("get-notifications")]
        public async Task<ActionResult> GetAll()
        {
            var data = await _context.Notifications.ToListAsync();
            return Ok(data);
        }




        [HttpPost]
        [Route("get-notifications-user")]
        public async Task<ActionResult> GetNotificationsUser([FromForm] string userId)
        {
            var data = await _context.Notifications.Where(x => x.UserId == userId || x.UserId =="user" ).ToListAsync();
            return Ok(data);
        }


        [HttpGet]
        [Route("get-notifications-admin")]
        public async Task<ActionResult> GetNotificationsAdmin([FromForm] string userId)
        {
            var data = await _context.Notifications.Where(x =>x.UserId == userId ).ToListAsync();
            return Ok(data);
        }


        //     [HttpPost]
        // [Route("add-notification")]
        // public async Task<ActionResult> AddNotey([FromForm]CreateNotificationDto notification)
        // {
        //     var data = await _context.Notifications.AddAsync(notification);
        //      await _context.SaveChangesAsync();
        //     return Ok(data);
        // }



    }
}