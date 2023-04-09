using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace awamrakeApi.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Lable { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public DateTime CreatedAt { get; set; }

        public Address()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
