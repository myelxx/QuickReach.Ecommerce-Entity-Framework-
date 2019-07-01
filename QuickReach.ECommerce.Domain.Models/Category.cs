using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Domain.Models
{
    [Table("Category")]
    public class Category : EntityBase
    {
        public Category()
        {
            this.ChildCategories = new List<CategoryRollup>();
            this.ProductCategories = new List<ProductCategory>();
            this.ParentCategories = new List<CategoryRollup>();
        }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        public IEnumerable<ProductCategory> ProductCategories { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<CategoryRollup> ChildCategories { get; set; }
        public IEnumerable<CategoryRollup> ParentCategories { get; set; }

        public void AddChild(int categoryId)
        {
            if (this.ID == categoryId)
            {
                throw new ArgumentException("Child category ID must not be the same as the parent category id");
            }

            // check if category id exists
            // check if category id is not a child yet

            var child = new CategoryRollup()
            {
                ParentCategoryID = this.ID,
                ChildCategoryID = categoryId
            };
            ((ICollection<CategoryRollup>)this.ChildCategories).Add(child);
        }

        public CategoryRollup GetChildCategory(int childId)
        {
            return ((ICollection<CategoryRollup>)this.ChildCategories)
                                                     .FirstOrDefault( pc => pc.ChildCategoryID == childId);
        }

        public void RemoveChildCategory(int productId)
        {
            var child = this.GetChildCategory(productId);
            ((ICollection<CategoryRollup>)this.ChildCategories).Remove(child);
        }

        public void AddProduct(ProductCategory child)
        {
            ((ICollection<ProductCategory>)this.ProductCategories).Add(child);
        }

        public ProductCategory GetProduct(int productId)
        {
            return ((ICollection<ProductCategory>)this.ProductCategories)
                                                      .FirstOrDefault(pc => pc.CategoryID == this.ID &&
                                                                            pc.ProductID == productId);
        }
        public void RemoveProduct(int productId)
        {
            var child = this.GetProduct(productId);
            ((ICollection<ProductCategory>)this.ProductCategories).Remove(child);
        }

    }
}
