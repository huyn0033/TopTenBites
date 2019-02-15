using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.Api.v1.Controllers;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;
using Xunit;

namespace TopTenBites.Tests.Api.v1
{
    public class LikesControllerUnitTests
    {
        [Fact]
        public void GetAllByBusinessId_BusinessId_ExpectOk()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };

            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockLikeService = new Mock<ILikeService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<Like>, IEnumerable<LikeApiModel>>(It.IsAny<IEnumerable<Like>>()))
                      .Returns(new List<LikeApiModel>() { new LikeApiModel { Id = 1 } });

            // Act
            var likesController = new LikesController(mockBusinessService.Object, mockMenuItemService.Object, mockLikeService.Object, mockMapper.Object);
            var actualResult = likesController.GetAllByBusinessId(business.BusinessId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<IEnumerable<Like>, IEnumerable<LikeApiModel>>(It.IsAny<IEnumerable<Like>>()), Times.Once);
        }

        [Fact]
        public void GetAllByBusinessId_BusinessIdNotFound_ExpectNotFound()
        {
            // Arrange
            var business = new Business { BusinessId = -1 };

            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockLikeService = new Mock<ILikeService>();
            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns((Business)null);

            // Act
            var likesController = new LikesController(mockBusinessService.Object, mockMenuItemService.Object, mockLikeService.Object, mockMapper.Object);
            var actualResult = likesController.GetAllByBusinessId(business.BusinessId);
           
            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }

        [Fact]
        public void GetAllByMenuItemId_MenuItemId_ExpectOk()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = 1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockLikeService = new Mock<ILikeService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns(menuItem);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<Like>, IEnumerable<LikeApiModel>>(It.IsAny<IEnumerable<Like>>()))
                      .Returns(new List<LikeApiModel>() { new LikeApiModel { Id = 1 } });

            // Act
            var likesController = new LikesController(mockBusinessService.Object, mockMenuItemService.Object, mockLikeService.Object, mockMapper.Object);
            var actualResult = likesController.GetAllByMenuItemId(menuItem.MenuItemId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<IEnumerable<Like>, IEnumerable<LikeApiModel>>(It.IsAny<IEnumerable<Like>>()), Times.Once);
        }

        [Fact]
        public void GetAllByMenuItemId_MenuItemIdNotFound_ExpectNotFound()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = -1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockLikeService = new Mock<ILikeService>();
            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns((MenuItem)null);

            // Act
            var likesController = new LikesController(mockBusinessService.Object, mockMenuItemService.Object, mockLikeService.Object, mockMapper.Object);
            var actualResult = likesController.GetAllByBusinessId(menuItem.MenuItemId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }

        [Fact]
        public void Get_LikeId_ExpectOk()
        {
            // Arrange
            var like = new Like { LikeId = 1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService.Setup(x => x.Get(It.IsAny<int>())).Returns(like);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Like, LikeApiModel>(It.IsAny<Like>()))
                      .Returns(new LikeApiModel { Id = 1 });

            // Act
            var likesController = new LikesController(mockBusinessService.Object, mockMenuItemService.Object, mockLikeService.Object, mockMapper.Object);
            var actualResult = likesController.Get(like.LikeId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<Like, LikeApiModel>(It.IsAny<Like>()), Times.Once);
        }

        [Fact]
        public void Get_LikeIdNotFound_ExpectNotFound()
        {
            // Arrange
            var like = new Like { LikeId = -1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockMapper = new Mock<IMapper>();
            var mockLikeService = new Mock<ILikeService>();
            mockLikeService.Setup(x => x.Get(It.IsAny<int>())).Returns((Like)null);

            // Act
            var likesController = new LikesController(mockBusinessService.Object, mockMenuItemService.Object, mockLikeService.Object, mockMapper.Object);
            var actualResult = likesController.Get(like.LikeId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
    }
}
