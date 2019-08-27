using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPets.Domain.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        [Range(minimum: 0.01, maximum: (double) decimal.MaxValue)]
        public decimal Price { get; set; }

        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}