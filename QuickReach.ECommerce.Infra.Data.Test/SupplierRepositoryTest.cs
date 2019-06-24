using QuickReach.ECommerce.Domain.Models;
using QuickReach.ECommerce.Infra.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;

namespace QuickReach.ECommerce.Infra.Data.Test
{

    public class SupplierRepositoryTest
    {
        public void Create_WithValidEntity_ShouldSaveInDatabase()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            var supplier = new Supplier {
                Name = "Melrose Mejidana",
                Description = "Shoe Supplier",
                IsActive = true
            };

            //Act
            sut.Create(supplier);

            //Assert
            Assert.True(supplier.ID != 0);

            var actual = sut.Retrieve(supplier.ID);
            Assert.NotNull(actual);

            //Clenup
            sut.Delete(supplier.ID);
        }

        [Fact]
        public void Retreieve_WithValidEntity_ShouldReturnValidEntity()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            var supplier = new Supplier {
                Name = "Melrose Mejidana",
                Description = "Shoe Supplier",
                IsActive = true
            };

            //Act
            var actual = sut.Retrieve(supplier.ID);

            //Assert
            Assert.NotNull(actual);

            //Cleanup
            sut.Delete(supplier.ID);
        }

        [Fact]
        public void Retrieve_WithInvalidEntity_ShouldReturnNull()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);

            //Act
            var actual = sut.Retrieve(-1);

            //Assert
            Assert.Null(actual);
        }

        public void Retrieve_WithSkipAndCount_ShouldReturnValidList()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            
            for(var i=1; i <= 20; i += 1)
            {
                sut.Create(new Supplier {
                    Name = string.Format("Supplier {0}", i),
                    Description = string.Format("Description {0}", i),
                    IsActive = true
                });
            }

            //Act
            var list = sut.Retrieve(5, 5);

            //Assert
            Assert.True(list.Count() == 5);

            //Cleanup
            list = sut.Retrieve(0, int.MaxValue).ToList();
            foreach( var entity in list)
            {
                sut.Delete(entity.ID);
            }



        }

        [Fact]
        public void Update_WithValidEntity_ShouldUpdateDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            var supplier = new Supplier
            {
                Name = "Melrose Mejidana",
                Description = "Shoe Supplier",
                IsActive = true
            };

            Assert.True(supplier.ID != 0);

            supplier.Name = "Myel Mejidana";
            supplier.Description = "Shoe Supplier will be hiatus";
            supplier.IsActive = false;

            //Act
            sut.Update(supplier.ID, supplier);
            var actual = sut.Retrieve(supplier.ID);

            //Assert
            Assert.Equal(supplier.Name, actual.Name);
            Assert.Equal(supplier.Description, actual.Description);
            Assert.Equal(supplier.IsActive, actual.IsActive);

            //Cleanup
            sut.Delete(supplier.ID);
        }

        public void Delete_WithValidEntity_ShouldRemoveDatabaseRecord()
        {
            //Arrange
            var context = new ECommerceDbContext();
            var sut = new SupplierRepository(context);
            var supplier = new Supplier
            {
                Name = "Melrose Mejidana",
                Description = "Shoe Supplier",
                IsActive = true
            };

            //Act
            sut.Delete(supplier.ID);
            var actual = sut.Retrieve(supplier.ID);

            //Assert
            Assert.Null(actual);
        }
    }
}
