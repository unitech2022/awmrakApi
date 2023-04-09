

using System.Security.Claims;
using System.Threading.Tasks;
using awamrakeApi.Models;
using awamrakeApi.Data;
using Microsoft.AspNetCore.Http;
using awamrakeApi.Notification;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System;
using System.IO;

namespace awamrakeApi.Helpers{



    class Functions {


           public static async Task<User> getCurrentUser(IHttpContextAccessor _httpContextAccessor, AwamrakeApiContext _context)
    {
        var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        var user = await _context.Users.FindAsync(userId);
        return user;
    }

       public static NotificationData SendNotificationFromFirebaseCloud([FromForm] NotificationData data)
        {
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", "AAAA835VwYI:APA91bFBUJcvyacL4hB3jUyI2UqwD4zFjpwp_13rK9VbI6iUX-myo1T7Q6UP1a6bONzViRS0VSLTQtdXKkRPGJR5OF54Vq_lFEUk-jyYiWGEYQ2d1spu83RPBolahOrXn3iHEPUGAd3p"));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", "1045796602242"));
            tRequest.ContentType = "application/json";
            var payload = new
            {
                to = "/topics/admin",
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

            return data;
        }


    }
    }
