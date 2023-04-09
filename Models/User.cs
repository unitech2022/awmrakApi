using System;
using Microsoft.AspNetCore.Identity;

namespace awamrakeApi.Models
{
   public class User : IdentityUser
    {
      
        public string FullName { get; set; }
        public string Status { get; set; }
        public string DeviceToken { get; set; }
        public string ImageUrl { get; set; }

        public string Code { get; set; }
        public string Role { get; set; }
        public double Reviewsum { get; set; }
        public double Reviews { get; set; }
        public double Rate { get; set; }

        public DateTime CreatedAt { get; set; }
        public User() {
           CreatedAt = DateTime.Now;
        }
    
    }
}