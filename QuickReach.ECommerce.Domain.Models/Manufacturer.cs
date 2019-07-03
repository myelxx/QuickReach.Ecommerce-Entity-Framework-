using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Manufacturer")]
    public class Manufacturer : EntityBase
    {
        public Manufacturer()
        {
            this.ProductManufacturers = new List<ProductManufacturer>();
        }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public IEnumerable<ProductManufacturer> ProductManufacturers { get; set; }

        public void AddProduct(ProductManufacturer child)
        {
            ((ICollection<ProductManufacturer>)this.ProductManufacturers).Add(child);
        }

        public ProductManufacturer GetProduct(int productId)
        {
            return ((ICollection<ProductManufacturer>)this.ProductManufacturers)
                                                      .FirstOrDefault(pc => pc.ManufacturerID == this.ID &&
                                                                            pc.ProductID == productId);
        }
        public void RemoveProduct(int productId)
        {
            var child = this.GetProduct(productId);
            ((ICollection<ProductManufacturer>)this.ProductManufacturers).Remove(child);
        }
    }
}
