using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repository
{
    public class SupplierRepository :  RepositoryBase<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ECommerceDbContext context) : base (context) //calls and pass context to contructor of repository base
        {

        }

        public IEnumerable<Supplier> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Suppliers
                        .Where(c => c.Name.Contains(search)
                               || c.Description.Contains(search))
                       .Skip(skip)
                       .Take(count)
                       .ToList();

            return result;
        }

        public override Supplier Retrieve(int entityId)
        {
            var entity = this.context.Suppliers
                             .Include(s => s.ProductSuppliers)
                             .Where(c => c.ID == entityId)
                             .FirstOrDefault();

            return entity;
        }

    }
}
