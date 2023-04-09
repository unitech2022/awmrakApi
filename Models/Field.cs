using System;
namespace awamrakeApi.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string Image { get; set; }


         public double Lat { get; set; }

         public double Lng { get; set; }

         public string Address { get; set; }

        public string phone { get; set; }
         public string Details { get; set; }
        public string UserId { get; set; }
        public string City { get; set; }

         public string Token { get; set; }
        public int status { get; set; }

        public int CategoryId { get; set; }

        public Field()
        {
            status = 0;
        }
    }

}
