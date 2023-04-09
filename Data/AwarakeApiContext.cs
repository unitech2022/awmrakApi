

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using awamrakeApi.Models;
using awamrakeApi.Controllers;

namespace awamrakeApi.Data
{



    public class AwamrakeApiContext : IdentityDbContext<User>
    {

        public AwamrakeApiContext(DbContextOptions<AwamrakeApiContext> otp) : base(otp)
        {

        }







        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

           public DbSet<Slider> Sliders { get; set; }

          public DbSet<Field> Fields { get; set; }

            public DbSet<Cart> Carts { get; set; }

          public DbSet<Order> Orders { get; set; }

        //     public DbSet<Post> Posts { get; set; }

        //     public DbSet<Comment> Comments { get; set; }
          public DbSet<Sitting> Sittings { get; set; }
            public DbSet<Favorite> Favorites { get; set; }

           public DbSet<Address> Addresses { get; set; }

          public DbSet<Notifications> Notifications { get; set; }

          public DbSet<Care> Cares { get; set; }


        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Driver_Field> Driver_Fields { get; set; }
        public DbSet<Driver_Order> Driver_Orders { get; set; }

        internal object Where(Func<object, object> value)
        {
            throw new NotImplementedException();
        }

    }
}