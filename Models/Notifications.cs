
using System;
namespace awamrakeApi.Models
{
    public class Notifications
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Image { get; set; }
        public int Status { get; set; }
        public string UserId { get; set; }
         public string Body { get; set; }
         public DateTime CreatedAt { get; set; }

         
        public Notifications()
        {
           
           CreatedAt = DateTime.Now;

         Status =0;  
        }
    }

}
