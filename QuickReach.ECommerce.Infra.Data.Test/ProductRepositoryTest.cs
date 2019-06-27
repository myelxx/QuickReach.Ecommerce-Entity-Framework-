using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Collections;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Infra.Data.Test.Utilities;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class ProductRepositoryTest
    {
        #region Create
        [Fact]
        public void Create_WithValidEntity_ShouldCreateNewDatabaseRecord()
        {
            //Arrange
            var options = TestHelper.Sqlite();

            var category = new Category();
            var expected = new Product();

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                //create category
                category = TestHelper.SampleCategory();
                context.Categories.Add(category);
                context.SaveChanges();

                //create product
                expected = TestHelper.SampleProduct(category.ID);
                var sut = new ProductRepository(context);

                //add product category to product instance
                ProductCategory productCategory = new ProductCategory
                {
                    CategoryID = category.ID,
                    Category = category,
                    ProductID = expected.ID,
                    Product = expected
                };

                List<ProductCategory> productCategoryList = new List<ProductCategory>();
                productCategoryList.Add(productCategory);
                expected.ProductCategories = productCategoryList;

                //Act
                sut.Create(expected);
            }

            using(var context = new ECommerceDbContext(options))
            {
                var actual = context.Products.Find(expected.ID);

                //Assert
                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
                Assert.Equal(expected.Price, actual.Price);
                Assert.Equal(expected.ImgURL, actual.ImgURL);
            }
        }
        #endregion

        #region Retrieve Valid Entity
        [Fact]
        public void Retrieve_WithValidEntityID_ReturnsAValidEntity()
        {
            //Arrange
            var options = TestHelper.Sqlite();

            var category = new Category();
            var expected = new Product();

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                //create category
                category = TestHelper.SampleCategory();
                context.Categories.Add(category);
                context.SaveChanges();

                //create product
                expected = TestHelper.SampleProduct(category.ID);

                context.SaveChanges();
            }
 
            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Products.Add(expected);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                // Act
                var actual = sut.Retrieve(expected.ID);

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
                Assert.Equal(expected.Price, actual.Price);
                Assert.Equal(expected.ImgURL, actual.ImgURL);
            }
        }
        #endregion

        #region Retrieve Invalid Entity
        [Fact]
        public void Retrieve_WithRetrieve_WithNonExistingEntityID_ReturnsNull()
        {
            var options = TestHelper.Sqlite();

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Arrange
                var sut = new ProductRepository(context);

                // Act
                var actual = sut.Retrieve(0);

                // Assert
                Assert.Null(actual);
            }
        }
        #endregion

        #region Retrieve With Skip & Count
        [Theory]
        [InlineData(0, 5)]
        [InlineData(10, 5)]
        [InlineData(15, 5)]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage(int skip, int count)
        {
            var options = TestHelper.Sqlite();

            var category = new Category();
            var expected = new Product();

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                //create category
                category = TestHelper.SampleCategory();
                context.Categories.Add(category);
                context.SaveChanges();

                // Arrange
                for (var i = 1; i <= 20; i += 1)
                {
                    //create product
                    context.Products.Add(TestHelper.SampleProduct(category.ID));
                }

                context.SaveChanges();

            }
            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                // Act & Assert
                var list = sut.Retrieve(skip, count);
                Assert.True(list.Count() == count);

                list = sut.Retrieve(20, 5);
                Assert.True(list.Count() == 0);

                Assert.NotNull(list);
            }
        }
        #endregion

        #region Update
        [Fact]
        public void Update_WithValidEntity_ShouldReflectChangesInDatabaseRecord()
        {
            var options = TestHelper.Sqlite();

            //to update 
            var expectedName = "Black Boots";
            var expectedDescription = "Black boots for sale";
            var expectedPrice = 1200;
            var expectedImgURL = "black_boots_classA.png";
            int expectedId = 0;

            var category = new Category();
            var expected = new Product();

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                //create category
                category = TestHelper.SampleCategory();
                context.Categories.Add(category);
                context.SaveChanges();

                // Arrange
                //create product
                var entity = TestHelper.SampleProduct(category.ID);
                context.Products.Add(entity);
                context.SaveChanges();

                expectedId = entity.ID;
            }

            using (var context = new ECommerceDbContext(options))
            {
                // Arrange
                var entity = context.Products.Find(expectedId);

                entity.Name = expectedName;
                entity.Description = expectedDescription;
                entity.ImgURL = expectedImgURL;
                entity.Price = expectedPrice;

                var sut = new ProductRepository(context);

                // Act
                sut.Update(entity.ID, entity);
                var actual = context.Products.Find(entity.ID);

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedDescription, actual.Description);
                Assert.Equal(expectedPrice, actual.Price);
                Assert.Equal(expectedImgURL, actual.ImgURL);
            }
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_WithValidEntity_ShouldDeleteDatabaseRecord()
        {
            var options = TestHelper.Sqlite();

            var category = new Category();
            var entity = new Product();

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                //create category
                category = TestHelper.SampleCategory();

                context.Categories.Add(category);
                context.SaveChanges();

                //create product
                entity = TestHelper.SampleProduct(category.ID);

                // Arrange
                context.Products.Add(entity);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new ProductRepository(context);

                // Act
                sut.Delete(entity.ID);
                entity = context.Products.Find(entity.ID);

                // Assert
                Assert.Null(entity);
            }
        }
        #endregion

        //#region Create Category Throws Exception
        //[Fact]
        //public void Create_WithValidProductAndWithoutExistingCategory_ShouldThrowException()
        //{
        //    //Arrange
        //    var options = TestHelper.Sqlite();

        //    var expected = new Product();

        //    using (var context = new ECommerceDbContext(options))
        //    {
        //        context.Database.OpenConnection();
        //        context.Database.EnsureCreated();

        //        //create product
        //        expected = TestHelper.SampleProduct(-1);
        //        var sut = new ProductRepository(context);

        //        //Act & assert
        //        Assert.Throws<DbUpdateException>(() => sut.Create(expected));
        //    }

        //}
        //#endregion
    }
}
