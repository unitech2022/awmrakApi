using System.ComponentModel.DataAnnotations;

namespace awamrakeApi.Models{


    public class Slider{


         [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }
    }
}