using Microsoft.EntityFrameworkCore;
using ContosoPets.Domain.Models;

namespace ContosoPets.DataAccess.Data
{
    public partial class ContosoPetsContext : DbContext
    {
        public ContosoPetsContext(DbContextOptions<ContosoPetsContext> options)
            : base(options)
        {
        }

       public DbSet<Customer> Customers { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductOrder> ProductOrders { get; set; }
    }
}
