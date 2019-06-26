using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Entity_Configuration
{
    class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Prdct");

            builder.Property(p => p.ID)
                   .IsRequired()
                   .ValueGeneratedOnAdd(); 

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products);
        }
    }
}
