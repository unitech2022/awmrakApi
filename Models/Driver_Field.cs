using System;
namespace awamrakeApi.Models
{
    public class Driver_Field
    {

        public int Id { get; set; }
        public int DriverId { get; set; }
        public int FieldId { get; set; }

        public DateTime CreatedAt { get; set; }

        public Driver_Field()
        {
            CreatedAt = DateTime.Now;
        }
    }
}
