using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Test.Utilities
{
    public static class TestHelper
    {
        
        public static DbContextOptions<ECommerceDbContext> Sqlite()
        {
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                        .UseSqlite(connection)
                        .Options;

            return options;
        }

        public static Category SampleCategory()
        {
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            return category;
        }

        public static Product SampleProduct(this int categoryId)
        {
            var product = new Product
            {
                Name = "Boots",
                Description = "Boots for sell",
                Price = 1500,
                CategoryID = categoryId,
                ImgURL = "sample_boots_1.png"
            };

            return product;
        }

    }
}
