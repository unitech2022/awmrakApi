using System.ComponentModel.DataAnnotations;

namespace awamrakeApi.Models
{

    public class CreateCartDto
    {



        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public string Image { get; set; }



        public double Price { get; set; }

        public double Total { get; set; }
        public string NameProduct { get; set; }


        public string UserId { get; set; }


        public int market_id { get; set; }
        public int Quantity { get; set; }
    }
}
// Carts
// id orderId(int) userId productId(int) quantity(int)