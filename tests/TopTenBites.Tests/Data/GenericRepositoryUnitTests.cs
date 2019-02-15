using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.Data;
using TopTenBites.Web.Data.Repositories;
using Xunit;

namespace TopTenBites.Tests.Data
{
    public class GenericRepositoryUnitTests
    {
        private Mock<ApplicationDbContext> _mockApplicationDbContext;
        public GenericRepositoryUnitTests()
        {
            var mockOptions = new Mock<DbContextOptions<ApplicationDbContext>>();
            mockOptions.SetupGet(x => x.ContextType).Returns(typeof(ApplicationDbContext));
            _mockApplicationDbContext = new Mock<ApplicationDbContext>(mockOptions.Object);
        }

        [Fact]
        public void Add_Entity_ExpectSuccess()
        {
            // Arrange
            var newBusiness = new Business() { BusinessId = 1, YelpBusinessId = "yelp1" };

            var mockDbSet = new Mock<DbSet<Business>>();

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);
            _mockApplicationDbContext.Setup(x => x.SaveChanges());
            mockDbSet.Setup(x => x.Add(It.IsAny<Business>()));

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            var actualBusiness = repository.Add(newBusiness);

            // Assert
            mockDbSet.Verify(x => x.Add(It.IsAny<Business>()));
            _mockApplicationDbContext.Verify(x => x.SaveChanges());
            Assert.Equal(newBusiness.YelpBusinessId, actualBusiness.YelpBusinessId);
        }

        [Fact]
        public void Update_Entity_ExpectSuccess()
        {
            // Arrange
            var business = new Business() { BusinessId = 1, YelpBusinessId = "yelp1" };

            _mockApplicationDbContext.Setup(x => x.SetModified(It.IsAny<Business>()));
            _mockApplicationDbContext.Setup(x => x.SaveChanges());

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            repository.Update(business);

            // Assert
            _mockApplicationDbContext.Verify(x => x.SaveChanges());
        }

        [Fact]
        public void Remove_Entity_ExpectSuccess()
        {
            // Arrange
            var business = new Business() { BusinessId = 1, YelpBusinessId = "yelp1" };

            _mockApplicationDbContext.Setup(x => x.Remove(It.IsAny<Business>()));
            _mockApplicationDbContext.Setup(x => x.SaveChanges());

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            repository.Remove(business);

            // Assert
            _mockApplicationDbContext.Verify(x => x.Remove(It.IsAny<Business>()));
            _mockApplicationDbContext.Verify(x => x.SaveChanges());
        }

        [Fact]
        public void Remove_FilterFound_ExpectSuccess()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);
            _mockApplicationDbContext.Setup(x => x.SaveChanges());
            mockDbSet.Setup(x => x.Remove(It.IsAny<Business>()));

            var removeBusinessIds = new int[] { 1, 2 };

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            repository.Remove(x => removeBusinessIds.Contains(x.BusinessId));

            // Assert
            mockDbSet.Verify(x => x.Remove(It.IsAny<Business>()), Times.Exactly(2));
            _mockApplicationDbContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Remove_FilterNotFound_ExpectSuccess()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);
            _mockApplicationDbContext.Setup(x => x.SaveChanges());
            mockDbSet.Setup(x => x.Remove(It.IsAny<Business>()));

            var removeBusinessIds = new int[] { -1 };

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            repository.Remove(x => removeBusinessIds.Contains(x.BusinessId));

            // Assert
            mockDbSet.Verify(x => x.Remove(It.IsAny<Business>()), Times.Never);
            _mockApplicationDbContext.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public void RemoveById_BusinessIdFound_ExpectSuccess()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);
            mockDbSet.Setup(x => x.Find(It.IsAny<object>())).Returns((object[] i) => businesses.Where(y => y.BusinessId == (int)i[0]).SingleOrDefault());

            _mockApplicationDbContext.Setup(x => x.Remove(It.IsAny<Business>()));
            _mockApplicationDbContext.Setup(x => x.SaveChanges());

            var removedBusiness = businesses[2];

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            repository.RemoveById(removedBusiness.BusinessId);

            // Assert
            mockDbSet.Verify(x => x.Find(It.IsAny<object>()));
            _mockApplicationDbContext.Verify(x => x.Remove(It.IsAny<Business>()));
            _mockApplicationDbContext.Verify(x => x.SaveChanges());
        }

        [Fact]
        public void RemoveById_BusinessIdNotFound_ExpectSuccess()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);
            mockDbSet.Setup(x => x.Find(It.IsAny<object>()));

            _mockApplicationDbContext.Setup(x => x.Remove(It.IsAny<Business>()));
            _mockApplicationDbContext.Setup(x => x.SaveChanges());

            var removedBusinessId = -1;

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            repository.RemoveById(removedBusinessId);

            // Assert
            mockDbSet.Verify(x => x.Find(It.IsAny<object>()));
            _mockApplicationDbContext.Verify(x => x.Remove(It.IsAny<Business>()), Times.Never);
            _mockApplicationDbContext.Verify(x => x.SaveChanges(), Times.Never);
        }

        [Fact]
        public void GetById_BusinessIdFound_ExpectObject()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);
            mockDbSet.Setup(x => x.Find(It.IsAny<object>())).Returns((object[] i) => businesses.Where(y => y.BusinessId == (int)i[0]).SingleOrDefault());

            var expectedBusiness = businesses[2];

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            var actualBusiness = repository.GetById(expectedBusiness.BusinessId);

            // Assert
            mockDbSet.Verify(x => x.Find(It.IsAny<object>()));
            Assert.Equal(expectedBusiness.YelpBusinessId, actualBusiness.YelpBusinessId);
        }

        [Fact]
        public void GetById_BusinessIdNotFound_ExpectNull()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);
            mockDbSet.Setup(x => x.Find(It.IsAny<object>())).Returns((object[] i) => businesses.Where(y => y.BusinessId == (int)i[0]).SingleOrDefault());

            var expectedBusinessId = -1;

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            var actualBusiness = repository.GetById(expectedBusinessId);

            // Assert
            mockDbSet.Verify(x => x.Find(It.IsAny<object>()));
            Assert.Null(actualBusiness);
        }

        [Fact]
        public void Get_FilterFound_ExpectObject()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);

            var expectedBusiness = businesses[1];

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            var actualBusiness = repository.Get(x => x.BusinessId == expectedBusiness.BusinessId);

            // Assert
            Assert.Equal(expectedBusiness.YelpBusinessId, actualBusiness.YelpBusinessId);
        }

        [Fact]
        public void Get_FilterNotFound_ExpectNull()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);

            var expectedBusinessId = -1;

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            var actualBusiness = repository.Get(x => x.BusinessId == expectedBusinessId);

            // Assert
            Assert.Null(actualBusiness);
        }

        [Fact]
        public void GetAll_FilterFound_ExpectObjects()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);

            var expectedBusinessIds = new int[] { 1, 3 };

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            var actualBusinesses = repository.GetAll(x => expectedBusinessIds.Contains(x.BusinessId));

            // Assert
            Assert.Equal(expectedBusinessIds.Count(), actualBusinesses.Count());
        }

        [Fact]
        public void GetAll_FilterNotFound_ExpectNull()
        {
            // Arrange
            IList<Business> businesses = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };

            var mockDbSet = MockDbSetFactory.Create(businesses);

            _mockApplicationDbContext.Setup(x => x.Set<Business>()).Returns(mockDbSet.Object);

            var expectedBusinessIds = new int[] { -1 };

            // Act
            var repository = new GenericRepository<Business>(_mockApplicationDbContext.Object);
            var actualBusinesses = repository.GetAll(x => expectedBusinessIds.Contains(x.BusinessId));

            // Assert
            Assert.Empty(actualBusinesses);
        }
    }

    public static class MockDbSetFactory
    {
        // Creates a mock DbSet from the specified data.
        public static Mock<DbSet<T>> Create<T>(IEnumerable<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mock = new Mock<DbSet<T>>();
            mock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mock;
        }
    }


}
