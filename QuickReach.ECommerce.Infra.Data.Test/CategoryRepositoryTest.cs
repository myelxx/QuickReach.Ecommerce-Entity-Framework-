using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repository;
using System;
using Xunit;
using System.Collections;
using System.Linq;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class CategoryRepositoryTest
    {
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            //Act
            sut.Create(category);

            //Assert
            Assert.True(category.ID != 0);

            var entity = sut.Retrieve(category.ID);
            Assert.NotNull(entity);

            //Cleanup
            sut.Delete(category.ID);


        }

        [Fact]
        public void Retrieve_WithValidEntityID_ReturnsAValidEntity()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };
            var sut = new CategoryRepository(context);
            sut.Create(category);

            //Act
            var actual = sut.Retrieve(category.ID);

            //Assert
            Assert.NotNull(actual);

            //Clean up
            sut.Delete(category.ID);
        }

        [Fact]
        public void Retrieve_WithNonExistingEntityID_ReturnsNull()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);

            //Act
            var actual = sut.Retrieve(-1);

            //Assert
            Assert.Null(actual);

        }

        [Fact]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);

            for (var i = 1; i <= 20; i += 1)
            {
                sut.Create(new Category
                {
                    Name = string.Format("Category {0}", i),
                    Description = string.Format("Description {0}", i)
                });
            }

            //Act
            var list = sut.Retrieve(5, 5);

            //Assert
            Assert.True(list.Count() == 5);

            //Cleanup
            list = sut.Retrieve(0, Int32.MaxValue);
            list.All(c => { sut.Delete(c.ID); return true; });

            //count max, then --
        }

        [Fact]
        public void Delete_WithValidEntity_ShouldRemoveDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);

            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            sut.Create(category);
            sut.Retrieve(category.ID);

            Assert.True(category.ID != 0);

            //Act
            sut.Delete(category.ID);
            var actual = sut.Retrieve(category.ID);

            //Assert
            Assert.Null(actual);
        }

        [Fact]
        public void Update_WithValidEntity_ShouldUpdateDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new CategoryRepository(context);

            var category = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            sut.Create(category);

            category.Name = "Boots";
            category.Description = "Shoes Department Updated";

            var actual = sut.Retrieve(category.ID);

            //Act
            sut.Update(category.ID, category);
            var expected = sut.Retrieve(category.ID);

            //Assert
            Assert.Equal(actual.ID, expected.ID);
            Assert.Equal(actual.Name, expected.Name);
            Assert.Equal(actual.Description, expected.Description);

            //Cleanup
            sut.Delete(category.ID);
        }

    }
}
