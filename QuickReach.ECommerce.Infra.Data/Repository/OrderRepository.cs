using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(ECommerceDbContext context) : base(context)
        {
        }

        public override Order Retrieve(int entityId)
        {
            var entity = this.context.Orders
                            .Include(c => c.Items)
                            .Where(c => c.ID == entityId)
                            .FirstOrDefault();

            return entity;
        }

        public IEnumerable<Order> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            throw new NotImplementedException();
        }
    }
}
