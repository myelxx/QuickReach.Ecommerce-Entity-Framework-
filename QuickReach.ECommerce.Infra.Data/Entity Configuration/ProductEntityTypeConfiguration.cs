﻿using Microsoft.EntityFrameworkCore;
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
            builder.Property(p => p.ID)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            //builder.HasOne(c => c.Category)
            //       .WithMany(p => p.Products)
            //       .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne(p => p.Category)
            //       .WithMany(c => c.Products);
        }
    }
}
