using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Entity_Configuration
{
    class ProductSupplierEntityTypeConfiguration : IEntityTypeConfiguration<ProductSupplier>
    {
        public void Configure(EntityTypeBuilder<ProductSupplier> builder)
        {
            builder.ToTable("ProductSupplier");
            builder.HasKey(ps => new { ps.SupplierID, ps.ProductID });

            builder.HasOne(ps => ps.Supplier)
                   .WithMany(s => s.ProductSuppliers)
                   .HasForeignKey("SupplierID");

            builder.HasOne(ps => ps.Product)
                   .WithMany(p => p.ProductSuppliers)
                   .HasForeignKey("ProductID");
        }
    }
}
