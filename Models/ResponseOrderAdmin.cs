using System.Collections.Generic;

namespace awamrakeApi.Models
{


    public class ResponseOrderAdmin
    {



         public string UserPhone { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }
        public Order Order { get; set; }

        public Field Field { get; set; }

       
    }
}