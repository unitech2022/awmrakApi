// Products
// id sellerId name detail price(double) image categoryId(int) brandId(int) status(int)

using System.ComponentModel.DataAnnotations;

namespace awamrakeApi.Models
{

    public class Favorite
    {


        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string SellerId { get; set; }

        public string Name { get; set; }

        public string UserId { get; set; }


        public string Detail { get; set; }



        public double Price { get; set; }

        public string Image { get; set; }
        public string City { get; set; }
        public int CategoryId { get; set; }


        public int BrandId { get; set; }

        public int Status { get; set; }


    }


}