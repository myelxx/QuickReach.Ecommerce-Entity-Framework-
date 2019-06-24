using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Collections;
using System.Linq;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class ProductRepositoryTest
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateNewDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var product = new Product {
                Name = "Boots",
                Description = "Boots for sale",
                Price = 522,
                CategoryID = 121,
                ImgURL = "sample.png"
            };

            //Act
            sut.Create(product);

            //Assert
            Assert.True(product.ID != 0);

            //Cleanup
            sut.Delete(product.ID);
        }

        [Fact]
        public void Retrieve_WithValidEntityID_ReturnsAValidEntity()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var product = new Product
            {
                Name = "Boots",
                Description = "Boots for sale",
                Price = 522,
                CategoryID = 121,
                ImgURL = "sample.png"
            };
            sut.Create(product);

            //Act
            var actual = sut.Retrieve(product.ID);

            //Assert
            Assert.NotNull(actual);

            //Cleanup
            sut.Delete(product.ID);
        }

        [Fact]
        public void Retrieve_WithRetrieve_WithNonExistingEntityID_ReturnsNull()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);

            //Act
            var actual = sut.Retrieve(0);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);

            for(var i=1; i <= 20; i += 1)
            {
                sut.Create(new Product {
                    Name = string.Format("Product {0}", i),
                    Description = string.Format("Description {0}", i),
                    Price = 500 + i,
                    CategoryID = 121,
                    ImgURL = string.Format("img_url_sample_{0}.png", i)
                });
            }

            //Act
            var list = sut.Retrieve(5, 5);

            //Assert
            Assert.True(list.Count() == 5);

            //Cleanup
            list = sut.Retrieve(0, Int32.MaxValue);
            foreach (var entity in list)
            {
                sut.Delete(entity.ID);
            }
        }

        [Fact]
        public void Update_WithValidEntity_ShouldReflectChangesInDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var product = new Product {
                Name = "Boots",
                Description = "Boots for sale",
                Price = 522,
                CategoryID = 121,
                ImgURL = "sample.png"
            };

            sut.Create(product);
            product.Name = "Black Boots";
            product.Description = "Black boots is for sale right now";
            sut.Retrieve(product.ID);

            //Act
            sut.Update(product.ID, product);
            var actual = sut.Retrieve(product.ID);

            //Assert
            Assert.Equal(product.Name, actual.Name);
            Assert.Equal(product.Description, actual.Description);

            //Cleanup
            sut.Delete(product.ID);

        }

        [Fact]
        public void Delete_WithValidEntity_ShouldDeleteDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new ProductRepository(context);
            var product = new Product
            {
                Name = "Boots",
                Description = "Boots for sale",
                Price = 522,
                CategoryID = 121,
                ImgURL = "sample.png"
            };

            //Act
            sut.Delete(product.ID);
            var actual = sut.Retrieve(product.ID);

            //Assert
            Assert.Null(actual);
        }
    }
}
