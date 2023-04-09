// Products
// id sellerId name detail price(double) image categoryId(int) brandId(int) status(int)

using System;
using System.ComponentModel.DataAnnotations;

namespace awamrakeApi.Dto
{

    public class CreateProductDto
    {


        public string SellerId { get; set; }

        public string Name { get; set; }


        public string Detail { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }

        public string City { get; set; }

        public int CategoryId { get; set; }

        public bool IsSlider { get; set; }
        public int HomeCategoryId { get; set; }
        public int OfferId { get; set; }

        public double OfferPrice { get; set; }
        public int Status { get; set; }

        public DateTime CreateAt { get; set; }
    }
}