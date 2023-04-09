using System.Collections.Generic;

namespace awamrakeApi.Models
{


    public class ResponseOrder
    {



        public string UserPhone { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }
        public Order Order { get; set; }

        public Address Address { get; set; }

        public List<Cart> Products { get; set; }
    }
}