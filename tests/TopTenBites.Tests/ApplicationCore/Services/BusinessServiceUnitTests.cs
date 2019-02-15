using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TopTenBites.Web.ApplicationCore.Enums;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.ApplicationCore.Services;
using Xunit;

namespace TopTenBites.Tests.ApplicationCore.Services
{
    public class BusinessServiceUnitTests
    {
        [Fact]
        public void Get_YelpBusinessIdFound_ExpectObject()
        {
            // Arrange
            var business = new Business { BusinessId = 1, YelpBusinessId = "yelp1" };

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(), It.IsAny<string>())).Returns(business);

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.Get(business.YelpBusinessId);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(), It.IsAny<string>()));
            Assert.Equal(business.YelpBusinessId, actualBusiness.YelpBusinessId);
        }

        [Fact]
        public void GetByBusinessId_BusinessIdFound_ExpectObject()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(), It.IsAny<string>())).Returns(business);

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.GetByBusinessId(business.BusinessId);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(), It.IsAny<string>()));
            Assert.Equal(business.BusinessId, actualBusiness.BusinessId);
        }

        [Fact]
        public void GetAll_YelpBusinessIdsFilterEmpty_ExpectObjects()
        {
            // Arrange
            IList<Business> businesss = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };
            var yelpBusinessIds = string.Empty;

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Business, bool>>>(),
                It.IsAny<Func<IQueryable<Business>, IOrderedQueryable<Business>>>(),
                It.IsAny<string>()))
                         .Returns(businesss);

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusinesss = businessService.GetAll(yelpBusinessIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Business, bool>>>(),
                It.IsAny<Func<IQueryable<Business>, IOrderedQueryable<Business>>>(),
                It.IsAny<string>()));
            Assert.Equal(businesss.Count, actualBusinesss.Count());
        }

        [Fact]
        public void GetAll_YelpBusinessIdsFound_ExpectObjects()
        {
            // Arrange
            IList<Business> businesss = new List<Business>
            {
                new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };
            var yelpBusinessIds = "yelp1, yelp2";
            string[] aryYelpBusinessIds = yelpBusinessIds.Split(",")
                                                          .Select(x => x.Trim())
                                                          .Where(x => !string.IsNullOrWhiteSpace(x))
                                                          .ToArray();

            var expectedBusinesss = businesss.Where(x => aryYelpBusinessIds.Contains(x.YelpBusinessId));

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Business, bool>>>(),
                It.IsAny<Func<IQueryable<Business>, IOrderedQueryable<Business>>>(), 
                It.IsAny<string>()))
                         .Returns(expectedBusinesss);

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusinesss = businessService.GetAll(yelpBusinessIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Business, bool>>>(),
                It.IsAny<Func<IQueryable<Business>, IOrderedQueryable<Business>>>(),
                It.IsAny<string>()));

            Assert.Equal(expectedBusinesss.Count(), actualBusinesss.Count());
        }

        [Fact]
        public void GetAll_YelpBusinessIdsNotFound_ExpectNull()
        {
            // Arrange
            IList<Business> businesss = new List<Business>
            {
               new Business() { BusinessId = 1, YelpBusinessId = "yelp1" },
                new Business() { BusinessId = 2, YelpBusinessId = "yelp2" },
                new Business() { BusinessId = 3, YelpBusinessId = "yelp3" }
            };
            var yelpBusinessIds = "yelp123";
            string[] aryYelpBusinessIds = yelpBusinessIds.Split(",")
                                                          .Select(x => x.Trim())
                                                          .Where(x => !string.IsNullOrWhiteSpace(x))
                                                          .ToArray();

            var expectedBusinesss = businesss.Where(x => aryYelpBusinessIds.Contains(x.YelpBusinessId));

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Business, bool>>>(),
                It.IsAny<Func<IQueryable<Business>, IOrderedQueryable<Business>>>(),
                It.IsAny<string>()))
                         .Returns(expectedBusinesss);

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusinesss = businessService.GetAll(yelpBusinessIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Business, bool>>>(),
                It.IsAny<Func<IQueryable<Business>, IOrderedQueryable<Business>>>(),
                It.IsAny<string>()));

            Assert.Empty(actualBusinesss);
            Assert.Equal(expectedBusinesss.Count(), actualBusinesss.Count());
        }

        [Fact]
        public void Add_Entity_ExpectSuccess()
        {
            // Arrange
            var newBusiness = new Business() { BusinessId = 1, YelpBusinessId = "yelp1" };

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Add(It.IsAny<Business>())).Returns(newBusiness);

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.Add(newBusiness);

            // Assert
            mockRepository.Verify(x => x.Add(It.IsAny<Business>()));
            Assert.Equal(newBusiness.YelpBusinessId, actualBusiness.YelpBusinessId);
        }

        [Fact]
        public void Update_Entity_ExpectSuccess()
        {
            // Arrange
            var newBusiness = new Business() { BusinessId = 1, YelpBusinessId = "yelp1" };

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Update(It.IsAny<Business>()));

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            businessService.Update(newBusiness);

            // Assert
            mockRepository.Verify(x => x.Update(It.IsAny<Business>()));
        }

        [Fact]
        public void AddMenuItem_AddBusinessEntity_ExpectSuccess()
        {
            // Arrange
            var menuItemName = "menuItemName1";
            var yelpBusinessId = "yelp1";
            var yelpBusinessAlias = "yelpAlias1";

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                            It.IsAny<string>())).Returns((Business)null);
            mockRepository.Setup(x => x.Add(It.IsAny<Business>()));

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.AddMenuItem(menuItemName, yelpBusinessId, yelpBusinessAlias);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                             It.IsAny<string>()));
            mockRepository.Verify(x => x.Add(It.IsAny<Business>()));
            Assert.Equal(menuItemName, actualBusiness.MenuItems.ElementAt(0).Name);
        }

        [Fact]
        public void AddMenuItem_UpdateBusinessEntity_ExpectSuccess()
        {
            // Arrange
            var businessId = 1;
            var menuItemName = "menuItemName1";
            var yelpBusinessId = "yelp1";
            var yelpBusinessAlias = "yelpAlias1";
            var business = new Business() { BusinessId = businessId, YelpBusinessId = yelpBusinessId, YelpBusinessAlias = yelpBusinessAlias };

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                            It.IsAny<string>())).Returns(business);
            mockRepository.Setup(x => x.Update(business));

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.AddMenuItem(menuItemName, yelpBusinessId, yelpBusinessAlias);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                             It.IsAny<string>()));
            mockRepository.Verify(x => x.Update(business));
            Assert.Equal(menuItemName, actualBusiness.MenuItems.ElementAt(0).Name);
        }

        [Fact]
        public void AddLike_UpdateBusinessEntity_ExpectSuccess()
        {
            // Arrange
            var businessId = 1;
            int menuItemId = 1;
            var yelpBusinessId = "yelp1";
            var likeAction = LikeAction.Like;
            var fingerprintHash = "fingerprintHash1";
            var business = new Business() {
                BusinessId = businessId,
                YelpBusinessId = yelpBusinessId,
                MenuItems = new List<MenuItem> { new MenuItem { MenuItemId = menuItemId } }
            };

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                            It.IsAny<string>())).Returns(business);
            mockRepository.Setup(x => x.Update(business));

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.AddLike(likeAction, menuItemId, yelpBusinessId, fingerprintHash);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                             It.IsAny<string>()));
            mockRepository.Verify(x => x.Update(business));
            Assert.Equal(fingerprintHash, actualBusiness.MenuItems.ElementAt(0).Likes.ElementAt(0).UserFingerPrintHash);
        }

        [Fact]
        public void AddLike_UpdateBusinessEntityUserAlreadyLikedDisliked_ExpectNoUpdate()
        {
            // Arrange
            var businessId = 1;
            int menuItemId = 1;
            var yelpBusinessId = "yelp1";
            var likeAction = LikeAction.Like;
            var fingerprintHash = "fingerprintHash1";
            var business = new Business() { BusinessId = businessId, YelpBusinessId = yelpBusinessId };
            var menuItem = new MenuItem { MenuItemId = menuItemId };
            menuItem.Likes.Add(new Like { UserFingerPrintHash = fingerprintHash, IsLike = (likeAction == LikeAction.Like ? true : false) });
            business.MenuItems.Add(menuItem);

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                            It.IsAny<string>())).Returns(business);
            mockRepository.Setup(x => x.Update(business));

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.AddLike(likeAction, menuItemId, yelpBusinessId, fingerprintHash);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                             It.IsAny<string>()));
            mockRepository.Verify(x => x.Update(business), Times.Never);
        }

        [Fact]
        public void AddComment_UpdateBusinessEntity_ExpectSuccess()
        {
            // Arrange
            var businessId = 1;
            int menuItemId = 1;
            var yelpBusinessId = "yelp1";
            var text = "comment1";
            var commentSentiment = Sentiment.Positive;
            var business = new Business() { BusinessId = businessId, YelpBusinessId = yelpBusinessId };
            var menuItem = new MenuItem { MenuItemId = menuItemId };
            menuItem.Comments.Add(new Comment {  Text = text, Sentiment = commentSentiment });
            business.MenuItems.Add(menuItem);

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                            It.IsAny<string>())).Returns(business);
            mockRepository.Setup(x => x.Update(business));

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.AddComment(text, commentSentiment, menuItemId, yelpBusinessId);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                             It.IsAny<string>()));
            mockRepository.Verify(x => x.Update(business));
        }

        [Fact]
        public void AddImage_UpdateBusinessEntity_ExpectSuccess()
        {
            // Arrange
            var businessId = 1;
            int menuItemId = 1;
            var yelpBusinessId = "yelp1";
            var fileName = "fileName1";
            var business = new Business() { BusinessId = businessId, YelpBusinessId = yelpBusinessId };
            var menuItem = new MenuItem { MenuItemId = menuItemId };
            menuItem.Images.Add(new Image { Name = fileName });
            business.MenuItems.Add(menuItem);

            var mockRepository = new Mock<IGenericRepository<Business>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                            It.IsAny<string>())).Returns(business);
            mockRepository.Setup(x => x.Update(business));

            // Act
            var businessService = new BusinessService(mockRepository.Object);
            var actualBusiness = businessService.AddImage(fileName, menuItemId, yelpBusinessId);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Business, bool>>>(),
                                             It.IsAny<string>()));
            mockRepository.Verify(x => x.Update(business));
        }
    }
}
