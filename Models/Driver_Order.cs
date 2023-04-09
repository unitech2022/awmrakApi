using System;
namespace awamrakeApi.Models
{
    public class Driver_Order
    {
        public int Id { get; set; }

        public int DriverId { get; set; }
        public int OrderId { get; set; }
        public double DeliveryCost { get; set; }


        public DateTime CreatedAt { get; set; }

        public Driver_Order()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
