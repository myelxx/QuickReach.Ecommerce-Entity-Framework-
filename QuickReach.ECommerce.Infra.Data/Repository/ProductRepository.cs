using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IRepository<Product>
    {
        public ProductRepository(ECommerceDbContext context) : base(context) //calls contructor of repository base
        {

        }
    }
}
