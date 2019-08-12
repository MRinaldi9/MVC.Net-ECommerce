using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Entities;
using System.Data.Entity;

namespace SportsStore.Domain.Concrete
{
    class EFDbContext : DbContext
    {
        //Set the Entity Framework with the name property (Products) and the model of type Product which it will fill the rows of table Products
        public DbSet<Product> Products { get; set; }
    }
}
