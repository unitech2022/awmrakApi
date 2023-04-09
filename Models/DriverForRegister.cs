using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace awamrakeApi.Models
{
    public class DriverForRegister
    {
        public string FullName { get; set; }
        public string knownName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string IdentityNumber { get; set; }
        public string address { get; set; }
        public string City { get; set; }
        public string Field { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        //public string identityNumber { get; set; }
        public string IbanNumber { get; set; }
        public string ProfileImage { get; set; }
        public string carImage { get; set; }
        public string CompanyId { get; set; }
        public string identityImage { get; set; }
    }
}
