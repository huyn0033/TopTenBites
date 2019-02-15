using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using TopTenBites.Web.Api.v1.Controllers;
using TopTenBites.Web.ApplicationCore.Interfaces;
using AutoMapper;
using TopTenBites.Web.ApplicationCore.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TopTenBites.Web.Api.v1.ApiModels;

namespace TopTenBites.Tests.Api.v1
{
    public class BusinessesControllerUnitTests
    {
        [Fact]
        public void Get_ExpectOk()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };

            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<Business>, IEnumerable<BusinessApiModel>>(It.IsAny<IEnumerable<Business>>()))
                      .Returns(new List<BusinessApiModel>() { new BusinessApiModel { Id = 1, YelpBusinessId = "yelp1" } });

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Get();

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<IEnumerable<Business>, IEnumerable<BusinessApiModel>>(It.IsAny<IEnumerable<Business>>()), Times.Once);
        }

        [Fact]
        public void Get_BusinessId_ExpectOk()
        {
            // Arrange
            var business = new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" };

            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Business, BusinessApiModel>(It.IsAny<Business>()))
                      .Returns(new BusinessApiModel { Id = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" });

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Get(business.BusinessId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<Business, BusinessApiModel>(It.IsAny<Business>()), Times.Once);
        }

        [Fact]
        public void Get_BusinessIdNotFound_ExpectNotFound()
        {
            // Arrange
            var business = new Business { BusinessId = -1, YelpBusinessId = "yelp-1", YelpBusinessAlias = "yelp-1" };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMapper = new Mock<IMapper>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns((Business)null);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Get(business.BusinessId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }

        #region Post
        [Fact]
        public void Post_BusinessApiModel_ExpectOk()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { /*Id = 2,*/ YelpBusinessId = "yelp2", YelpBusinessAlias = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Post(businessVM);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<BusinessApiModel, Business>(It.IsAny<BusinessApiModel>()), Times.Once);
            mockMapper.Verify(x => x.Map<Business, BusinessApiModel>(It.IsAny<Business>()), Times.Once);
        }

        [Fact]
        public void Post_BusinessApiModelIdNotEmpty_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { Id = 2, YelpBusinessId = "yelp2", YelpBusinessAlias = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Post(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_BusinessApiModelYelpBusinessIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { /*Id = 2, YelpBusinessId = "yelp2"*/ YelpBusinessAlias = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Post(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_BusinessApiModelYelpBusinessAliasEmpty_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { /*Id = 2, YelpBusinessAlias = "yelp2"*/ YelpBusinessId = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Post(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_BusinessApiModelYelpBusinessIdDuplicate_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { /*Id = 2,*/ YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Post(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_BusinessApiModelYelpBusinessAliasDuplicate_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { /*Id = 2,*/ YelpBusinessId = "yelp2", YelpBusinessAlias = "yelp1" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Post(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }
        #endregion

        #region Put
        [Fact]
        public void Put_BusinessApiModel_ExpectOk()
        {
            // Arrange
            var business = new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" };
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { Id = 1, YelpBusinessId = "yelp2", YelpBusinessAlias = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);
            mockBusinessService.Setup(x => x.Update(It.IsAny<Business>()));

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Put(businessVM);

            // Assert
            Assert.IsType<OkResult>(actualResult);
            mockMapper.Verify(x => x.Map<BusinessApiModel, Business>(It.IsAny<BusinessApiModel>(), It.IsAny<Business>()), Times.Once);
        }

        [Fact]
        public void Put_BusinessApiModelIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { /*Id = 2,*/ YelpBusinessId = "yelp2", YelpBusinessAlias = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Put(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_BusinessApiModelYelpBusinessIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { Id = 1, /*YelpBusinessId = "yelp2"*/ YelpBusinessAlias = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Post(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_BusinessApiModelYelpBusinessAliasEmpty_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { Id = 1, YelpBusinessId = "yelp2"/*, YelpBusinessAlias = "yelp2"*/ };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Put(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_BusinessApiModelYelpBusinessIdDuplicate_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { Id = 2, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp2" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Put(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_BusinessApiModelYelpBusinessAliasDuplicate_ExpectBadRequest()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { Id = 2, YelpBusinessId = "yelp2", YelpBusinessAlias = "yelp1" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);

            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Put(businessVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_BusinessApiModelIdNotFound_ExpectNotFound()
        {
            // Arrange
            var businesses = new List<Business>() { new Business { BusinessId = 1, YelpBusinessId = "yelp1", YelpBusinessAlias = "yelp1" } };
            var businessVM = new BusinessApiModel() { Id = -1, YelpBusinessId = "yelp-1", YelpBusinessAlias = "yelp-1" };

            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetAll(It.IsAny<string>())).Returns(businesses);
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns((Business)null);
            
            // Act
            var businessesController = new BusinessesController(mockBusinessService.Object, mockMapper.Object);
            var actualResult = businessesController.Put(businessVM);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
        #endregion
    }
}
