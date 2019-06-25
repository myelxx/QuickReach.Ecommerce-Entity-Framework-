using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repository;
using System;
using Xunit;
using System.Collections;
using System.Linq;
using Microsoft.Data.Sqlite;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class CategoryRepositoryTest
    {
        #region Create
        [Fact]
        public void SQLiteCreate_WithValidEntity_ShouldCreateDatabaseRecord() 
        {
            //Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                        .UseSqlite(connection)
                        .Options;

            var expected = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new CategoryRepository(context);

                //Act
                sut.Create(expected);
            }

            using (var context = new ECommerceDbContext(options))
            {
                //Assert
                var actual = context.Categories.Find(expected.ID);

                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
            }

        }
        #endregion

        #region Retrieve Valid Entity
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
        #endregion

        #region Retrieve Non Existing Entity
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
        #endregion

        #region Retrieve With Skip & Count
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
            list = sut.Retrieve().ToList();
            foreach (var entity in list)
            {
                sut.Delete(entity.ID);
            }
            //list.All(c => { sut.Delete(c.ID); return true; }); 

        }
        #endregion

        #region Delete
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
        #endregion

        #region Update
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
        #endregion

    }
}
