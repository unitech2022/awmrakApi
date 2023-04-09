using System;
using System.ComponentModel.DataAnnotations;

namespace awamrakeApi.Models
{

    public class Product
    {

        [Key]
        public int Id { get; set; }
        public string SellerId { get; set; }

        public int HomeCategoryId { get; set; }

        public string Name { get; set; }

        public bool IsSlider { get; set; }
        public string Detail { get; set; }

        public double OfferPrice { get; set; }

        public double Price { get; set; }

        public string Image { get; set; }

        public string City { get; set; }

        public int CategoryId { get; set; }

        public int OfferId { get; set; }

        public int Status { get; set; }

        public DateTime CreateAt { get; set; }

        public Product()
        {

            CreateAt = DateTime.Now;
            OfferId = 0;
        }
    }
}