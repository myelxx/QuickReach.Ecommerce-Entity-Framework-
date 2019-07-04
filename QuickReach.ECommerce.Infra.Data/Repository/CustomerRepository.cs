using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repository
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(ECommerceDbContext context) : base(context)
        {
        }

        public IEnumerable<Customer> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            throw new NotImplementedException();
        }
    }
}
