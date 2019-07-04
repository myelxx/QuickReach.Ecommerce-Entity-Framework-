﻿using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Domain
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> Retrieve(string search = "", int skip = 0, int count = 10);
    }
}
