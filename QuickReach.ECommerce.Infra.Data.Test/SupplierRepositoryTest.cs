using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace QuickReach.ECommerce.Infra.Data.Test
{

    public class SupplierRepositoryTest
    {
        #region Create
        [Fact]
        public void Create_WithValidEntity_ShouldSaveInDatabase()
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

            var expected = new Supplier
            {
                Name = "Melrose Mejidana",
                Description = "Active shoe supplier"
            };

            using(var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var sut = new SupplierRepository(context);

                //Act
                sut.Create(expected);
            }

            using(var context = new ECommerceDbContext(options))
            {
                var actual = context.Supplier.Find(expected.ID);

                //Arrange
                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
            }

        }
        #endregion

        #region Retrieve Valid Entity
        [Fact]
        public void Retreieve_WithValidEntity_ShouldReturnValidEntity()
        {
            // Arrange
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                        .UseSqlite(connection)
                        .Options;

            var expected = new Supplier
            {
                Name = "Melrose Mejidana",
                Description = "Boots Supplier"
            };

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Supplier.Add(expected);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);

                //Act
                var actual = sut.Retrieve(expected.ID);

                //Assert
                Assert.NotNull(actual);
                Assert.Equal(expected.Name, actual.Name);
                Assert.Equal(expected.Description, actual.Description);
            }
        }
        #endregion

        #region Retrieve Invalid Entity
        [Fact]
        public void Retrieve_WithInvalidEntity_ShouldReturnNull()
        {
            var connectionBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = ":memory:"
            };
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                       .UseSqlite(connection)
                       .Options;

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Arrange
                var sut = new SupplierRepository(context);

                // Act
                var actual = sut.Retrieve(0);

                // Assert
                Assert.Null(actual);
            }
        }
        #endregion

        #region Retrieve With Skip & Count
        [Theory]
        [InlineData(0,5)]
        [InlineData(10, 5)]
        [InlineData(15, 5)]
        public void Retrieve_WithSkipAndCount_ShouldReturnCorrectPage(int skip, int count)
        {
            var connectionBuilder = new SqliteConnectionStringBuilder();
            var connection = new SqliteConnection(connectionBuilder.ConnectionString);
            var options = new DbContextOptionsBuilder<ECommerceDbContext>()
                        .UseSqlite(connection)
                        .Options;

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Arrange
                for (var i = 1; i <= 20; i += 1)
                {
                    context.Supplier.Add( new Supplier {
                        Name = string.Format("Supplier #{0}", i),
                        Description = string.Format("Description #{0}", i)
                    });
                }

                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);

                // Act and Assert
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

            //values to be update
            var expectedName = "Supplier Shoes";
            var expectedDescription = "Supplier Shoes Department";
            int expectedId = 0;

            using (var context = new ECommerceDbContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var entity = new Supplier
                {
                    Name = "Melrose Mejidana",
                    Description = "Active shoe supplier "
                };

                context.Supplier.Add(entity);
                context.SaveChanges();

                expectedId = entity.ID;
            }

            using (var context = new ECommerceDbContext(options))
            {
                // Arrange
                var entity = context.Supplier.Find(expectedId);

                entity.Name = expectedName;
                entity.Description = expectedDescription;

                var sut = new SupplierRepository(context);

                // Act
                sut.Update(entity.ID, entity);
                var actual = context.Supplier.Find(entity.ID);

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedDescription, actual.Description);
            }
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_WithValidEntity_ShouldRemoveDatabaseRecord()
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

            var entity = new Supplier
            {
                Name = string.Format("Supplier {0}", 1),
                Description = string.Format("Description {0}", 1)
            };

            using (var context = new ECommerceDbContext(options))
            {
                // Arrange
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                context.Supplier.Add(entity);
                context.SaveChanges();
            }

            using (var context = new ECommerceDbContext(options))
            {
                var sut = new SupplierRepository(context);

                // Act
                sut.Delete(entity.ID);
                entity = context.Supplier.Find(entity.ID);

                // Assert
                Assert.Null(entity);
            }
        } 
        #endregion
    }
}