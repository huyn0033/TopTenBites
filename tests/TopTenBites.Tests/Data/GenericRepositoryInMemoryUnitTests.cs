using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.Data;
using TopTenBites.Web.Data.Repositories;
using Xunit;
using System.Linq;

namespace TopTenBites.Tests.Data
{
    public class GenericRepositoryInMemoryUnitTests
    {
        [Fact]
        public void Add_Entity_ExpectSuccess()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "Add_Entity_ExpectSuccess");
            var businessId = 999;
            var newBusiness = new Business() { BusinessId = businessId, YelpBusinessId = "yelp999" };

            // Act: Run the test against one instance of the context
            int count;
            using (var context = new ApplicationDbContext(options))
            {
                count = context.Businesses.Count();

                var repository = new GenericRepository<Business>(context);
                var business = repository.Add(newBusiness);
            }

            // Assert: Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var business = context.Businesses.Where(x => x.BusinessId == businessId).SingleOrDefault();
                Assert.Equal(newBusiness.BusinessId, business?.BusinessId);
                Assert.Equal(count + 1, context.Businesses.Count());
            }
        }

        [Fact]
        public void Update_Entity_ExpectSuccess()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "Update_Entity_ExpectSuccess");
            var updateYelpBusinessId = "yelp123";

            // Act: Run the test against one instance of the context
            using (var context = new ApplicationDbContext(options))
            {
                var business = context.Businesses.Where(x => x.BusinessId == 1).Single();
                business.YelpBusinessId = updateYelpBusinessId;

                var repository = new GenericRepository<Business>(context);
                repository.Update(business);
            }

            // Assert: Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var business = context.Businesses.Where(x => x.BusinessId == 1).Single();
                Assert.Equal(updateYelpBusinessId, business.YelpBusinessId);
            }
        }
        [Fact]
        public void Remove_Entity_ExpectSuccess()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "Remove_Entity_ExpectSuccess");

            // Act: Run the test against one instance of the context
            int count;
            using (var context = new ApplicationDbContext(options))
            {
                count = context.Businesses.Count();
                var business = context.Businesses.Where(x => x.BusinessId == 1).Single();

                var repository = new GenericRepository<Business>(context);
                repository.Remove(business);
            }

            // Assert: Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                var business = context.Businesses.Where(x => x.BusinessId == 1).SingleOrDefault();
                Assert.Null(business);
                Assert.Equal(count - 1, context.Businesses.Count());
            }
        }

        [Fact]
        public void Remove_FilterFound_ExpectSuccess()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "Remove_FilterFound_ExpectSuccess");

            var removeBusinessIds = new int[] { 1, 2 };

            // Act: Run the test against one instance of the context
            int count;
            using (var context = new ApplicationDbContext(options))
            {
                count = context.Businesses.Count();

                var repository = new GenericRepository<Business>(context);
                repository.Remove(x => removeBusinessIds.Contains(x.BusinessId));
            }

            // Assert: Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(count - 2, context.Businesses.Count());
            }
        }

        [Fact]
        public void Remove_FilterNotFound_ExpectSuccess()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "Remove_FilterNotFound_ExpectSuccess");

            var removeBusinessIds = new int[] { -1 };

            // Act: Run the test against one instance of the context
            int count;
            using (var context = new ApplicationDbContext(options))
            {
                count = context.Businesses.Count();

                var repository = new GenericRepository<Business>(context);
                repository.Remove(x => removeBusinessIds.Contains(x.BusinessId));
            }

            // Assert: Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(count, context.Businesses.Count());
            }
        }

        [Fact]
        public void RemoveById_BusinessIdFound_ExpectSuccess()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "RemoveById_BusinessIdFound_ExpectSuccess");

            // Act: Run the test against one instance of the context
            int count;
            var removedBusinessId = 2;
            using (var context = new ApplicationDbContext(options))
            {
                count = context.Businesses.Count();

                var repository = new GenericRepository<Business>(context);
                repository.RemoveById(removedBusinessId);
            }

            // Assert: Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(count - 1, context.Businesses.Count());
            }
        }

        [Fact]
        public void RemoveById_BusinessIdNotFound_ExpectSuccess()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "RemoveById_BusinessIdNotFound_ExpectSuccess");

            // Act: Run the test against one instance of the context
            int count;
            var removedBusinessId = -1;
            using (var context = new ApplicationDbContext(options))
            {
                count = context.Businesses.Count();

                var repository = new GenericRepository<Business>(context);
                repository.RemoveById(removedBusinessId);
            }

            // Assert: Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(count, context.Businesses.Count());
            }
        }

        [Fact]
        public void GetById_BusinessIdFound_ExpectObject()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "GetById_BusinessIdFound_ExpectObject");

            // Act|Assert
            using (var context = new ApplicationDbContext(options))
            {
                var businessId = 2;

                var repository = new GenericRepository<Business>(context);
                var actualBusiness = repository.GetById(businessId);

                var expectedBusiness = context.Businesses.Where(x => x.BusinessId == businessId).Single();
                Assert.Equal(expectedBusiness.YelpBusinessId, actualBusiness.YelpBusinessId);
            }
        }

        [Fact]
        public void GetById_BusinessIdNotFound_ExpectNull()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "GetById_BusinessIdNotFound_ExpectNull");

            // Act|Assert
            using (var context = new ApplicationDbContext(options))
            {
                var businessId = -1;

                var repository = new GenericRepository<Business>(context);
                var actualBusiness = repository.GetById(businessId);

                Assert.Null(actualBusiness);
            }
        }

        [Fact]
        public void Get_FilterFound_ExpectObject()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "Get_FilterFound_ExpectObject");

            // Act|Assert
            using (var context = new ApplicationDbContext(options))
            {
                var businessId = 2;

                var repository = new GenericRepository<Business>(context);
                var actualBusiness = repository.Get(x => x.BusinessId == businessId);

                var expectedBusiness = context.Businesses.Where(x => x.BusinessId == businessId).Single();
                Assert.Equal(expectedBusiness.YelpBusinessId, actualBusiness.YelpBusinessId);
            }
        }

        [Fact]
        public void Get_FilterNotFound_ExpectNull()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "Get_FilterNotFound_ExpectNull");

            // Act|Assert
            using (var context = new ApplicationDbContext(options))
            {
                var businessId = -1;

                var repository = new GenericRepository<Business>(context);
                var actualBusiness = repository.Get(x => x.BusinessId == businessId);

                Assert.Null(actualBusiness);
            }
        }

        [Fact]
        public void GetAll_FilterFound_ExpectObjects()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "GetAll_FilterFound_ExpectObjects");

            // Act|Assert
            using (var context = new ApplicationDbContext(options))
            {
                var expectedBusinessIds = new int[] { 1, 3 };

                var repository = new GenericRepository<Business>(context);
                var actualBusinesses = repository.GetAll(x => expectedBusinessIds.Contains(x.BusinessId));

                Assert.Equal(expectedBusinessIds.Count(), actualBusinesses.Count());
            }
        }

        [Fact]
        public void GetAll_FilterNotFound_ExpectNull()
        {
            // Arrange
            var options = SetupAndSeedInMemoryDatabase(dbName: "GetAll_FilterNotFound_ExpectNull");

            // Act|Assert
            using (var context = new ApplicationDbContext(options))
            {
                var expectedBusinessIds = new int[] { -1 };

                var repository = new GenericRepository<Business>(context);
                var actualBusinesses = repository.GetAll(x => expectedBusinessIds.Contains(x.BusinessId));

                Assert.Empty(actualBusinesses);
            }
        }

        private DbContextOptions<ApplicationDbContext> SetupAndSeedInMemoryDatabase(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseInMemoryDatabase(databaseName: dbName)
                            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var businesses = new[] {
                    new Business { BusinessId = 1, YelpBusinessId = "yelp1" },
                    new Business { BusinessId = 2, YelpBusinessId = "yelp2" },
                    new Business { BusinessId = 3, YelpBusinessId = "yelp3" },
                };

                context.Businesses.AddRange(businesses);
                context.SaveChanges();
            }

            return options;
        }
    }
}
