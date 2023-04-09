using System;
using Newtonsoft.Json;

namespace awamrakeApi.Notification{


 public class NotificationData
    {
      
       
        
        public string Title { get; set; }
      
        public string Body { get; set; }

       
        public string Subject { get; set; }
       public string ImageUrl { get; set; }

        public string Desc { get; set; }

       public DateTime createAt{get;set;}
          public NotificationData()
        {
            createAt=DateTime.Now;
        }

    }

    public class Data
    {
        public Data()
        {
            createAt=DateTime.Now;
        }

        public string Subject { get; set; }
       public string ImageUrl { get; set; }

        public string Desc { get; set; }

       public DateTime createAt{get;set;}

       
        
       
    }


}