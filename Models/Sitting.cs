using System.ComponentModel.DataAnnotations;

namespace awamrakeApi.Models{


    public class Sitting{


         [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string value { get; set; }
    }
}