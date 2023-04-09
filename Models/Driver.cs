using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace awamrakeApi.Models
{
    public class Driver
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Bank { get; set; }
        public string City { get; set; }
        public string Field { get; set; }


        public string IBAN { get; set; }
        public double Balance { get; set; }

        public string UserId { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }

        public string IdentityNumber { get; set; }


        public DateTime CreatedAt { get; set; }

        public Driver()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
