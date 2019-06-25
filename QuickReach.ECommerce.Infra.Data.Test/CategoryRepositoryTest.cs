using Microsoft.EntityFrameworkCore;
using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repository;
using System;
using Xunit;
using System.Collections;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;

namespace QuickReach.ECommerce.Infra.Data.Test
{
    public class CategoryRepositoryTest
    {
        #region Create
        [Fact]
        public void Create_WithValidEntity_ShouldCreateDatabaseRecord() 
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

            using (var context = new ECommerceDbContext(options)) //context is applicable inside this using
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

            // Arrange
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                   .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                   .Options;

            var expected = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Categories.Add(expected);
                context.SaveChanges();

            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);

                // Act
                var actual = sut.Retrieve(expected.ID);

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
            }

        }
        #endregion

        #region Retrieve Non Existing Entity
        [Fact]
        public void Retrieve_WithNonExistingEntityID_ReturnsNull()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                        .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                        .Options;

            using (var context = new ECommerceDbContext(options))
            {
                // Arrange
                var sut = new CategoryRepository(context);

                // Act
                var actual = sut.Retrieve(0);

                // Assert
                Assert.Null(actual);
            }
        }
        #endregion

        #region Retrieve With Skip & Count
        [Fact]
        public void Retrieve_WithSkipAndCount_ReturnsTheCorrectPage()
        {
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                        .UseInMemoryDatabase($"CategoryForTesting{Guid.NewGuid()}")
                        .Options;

            using (var context = new ECommerceDbContext(options))
            {
                // Arrange
                for (var i = 1; i <= 20; i += 1)
                {
                    context.Categories.Add(new Category
                    {
                        Name = "Shoes",
                        Description = "Shoes Department"
                    });
                }
                context.SaveChanges();

            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);

                // Act & Assert
                var list = sut.Retrieve(5, 5);
                Assert.True(list.Count() == 5);

                list = sut.Retrieve(0, 5);
                Assert.True(list.Count() == 5);

                list = sut.Retrieve(10, 5);
                Assert.True(list.Count() == 5);

                list = sut.Retrieve(15, 5);
                Assert.True(list.Count() == 5);

                list = sut.Retrieve(20, 5);
                Assert.True(list.Count() == 0);
            }

        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_WithValidEntity_ShouldRemoveDatabaseRecord()
        {
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                        .UseSqlite(connection)
                        .Options;

            Category entity = new Category
            {
                Name = "Shoes",
                Description = "Shoes Department"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Arrange
                context.Categories.Add(entity);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new CategoryRepository(context);

                // Act
                sut.Delete(entity.ID);

                // Assert
                entity = context.Categories.Find(entity.ID);
                Assert.Null(entity);
            }
        }
        #endregion

        #region Update
        [Fact]
        public void Update_WithValidEntity_ShouldUpdateDatabaseRecord()
        {
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                        .UseSqlite(connection)
                        .Options;

            var expectedName = "Sandals";
            var expectedDescription = "Sandals Department";
            int expectedId = 0;

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Arrange
                var entity = new Category
                {
                    Name = "Shoes",
                    Description = "Shoes Department"
                };

                context.Categories.Add(entity);
                context.SaveChanges();

                expectedId = entity.ID;
            }

            using (var context = new ECommerceDbContext(options))
            {
                // Arrange
                var entity = context.Categories.Find(expectedId);

                entity.Name = expectedName;
                entity.Description = expectedDescription;

                var sut = new CategoryRepository(context);

                // Act
                sut.Update(entity.ID, entity);

                // Assert
                var actual = context.Categories.Find(entity.ID);

                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedDescription, actual.Description);
            }

        } 
        #endregion

    }
}
