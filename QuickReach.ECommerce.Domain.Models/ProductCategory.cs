using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
