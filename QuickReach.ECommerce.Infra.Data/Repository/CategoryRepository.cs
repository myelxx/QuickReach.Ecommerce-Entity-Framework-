using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QuickReach.ECommerce.Infra.Data.Repository
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ECommerceDbContext context) : base (context) //calls constructor of repositorybase
        {

        }

        //public override void Delete(int entityId)
        //{
        //    int productCount = this.context.Products.Where(p => p.CategoryID == entityId).Count();
        //    var category = Retrieve(entityId);

        //    if (productCount > 0)
        //    {
        //        throw new SystemException("This category can't be deleted. It will delete the products.");
        //    }

        //    this.context.Remove<Category>(category);
        //    this.context.SaveChanges();

        //}
        public override Category Retrieve(int entityId)
        {
            var entity = this.context.Categories
                         .AsNoTracking()
                         .Include(c => c.Products)
                         .Where(c => c.ID == entityId)
                         .FirstOrDefault();

            return entity;
        }

        public IEnumerable<Category> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Categories
                .Where(c => c.Name.Contains(search) ||
                            c.Description.Contains(search))
                .Skip(skip)
                .Take(count)
                .ToList();

            return result;
        }
    }
}
