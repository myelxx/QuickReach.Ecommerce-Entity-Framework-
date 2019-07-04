using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repository
{
    public class ManufacturerRepository : RepositoryBase<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(ECommerceDbContext context) : base(context)
        {
        }

        public IEnumerable<Manufacturer> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Manufacturers
                        .Where(c => c.Name.Contains(search)
                               || c.Description.Contains(search))
                       .Skip(skip)
                       .Take(count)
                       .ToList();

            return result;
        }

        public override Manufacturer Retrieve(int entityId)
        {
            var entity = this.context.Manufacturers
                             .Include(s => s.ProductManufacturers)
                             .Where(c => c.ID == entityId)
                             .FirstOrDefault();

            return entity;
        }
    }
}
