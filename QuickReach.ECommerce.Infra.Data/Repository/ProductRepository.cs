﻿using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ECommerceDbContext context) : base(context) //calls and pass context to contructor of repository base
        {

        }

        //public override Product Create(Product newEntity)
        //{
        //    this.context.Add(newEntity);

        //    ProductCategory productCategory = new ProductCategory()
        //    {
        //        CategoryID = 1,
        //        ProductID = newEntity.ID
        //    };

        //    this.context.ProductCategory.Add(productCategory);
        //    this.context.SaveChanges(); //to reflect in database
        //    return newEntity;
        //}

        public IEnumerable<Product> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Products
                        .Where(c => c.Name.Contains(search)
                               || c.Description.Contains(search))
                       .Skip(skip)
                       .Take(count)
                       .ToList();

            return result;
        }
    }
}
