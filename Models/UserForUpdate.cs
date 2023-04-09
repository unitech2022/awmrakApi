using System;
using Microsoft.AspNetCore.Http;

namespace awamrakeApi.Models
{
    public class UserForUpdate
    {
        public string FullName { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string DeviceToken { get; set; }
        public string ImageUrl { get; set; }
    }
}
