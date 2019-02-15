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
    public class ImagesControllerUnitTests
    {
        [Fact]
        public void GetAllByBusinessId_BusinessId_ExpectOk()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };

            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockImageService = new Mock<IImageService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<Image>, IEnumerable<ImageApiModel>>(It.IsAny<IEnumerable<Image>>()))
                      .Returns(new List<ImageApiModel>() { new ImageApiModel { Id = 1 } });

            // Act
            var imagesController = new ImagesController(mockBusinessService.Object, mockMenuItemService.Object, mockImageService.Object, mockMapper.Object);
            var actualResult = imagesController.GetAllByBusinessId(business.BusinessId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<IEnumerable<Image>, IEnumerable<ImageApiModel>>(It.IsAny<IEnumerable<Image>>()), Times.Once);
        }

        [Fact]
        public void GetAllByBusinessId_BusinessIdNotFound_ExpectNotFound()
        {
            // Arrange
            var business = new Business { BusinessId = -1 };

            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockImageService = new Mock<IImageService>();
            var mockMapper = new Mock<IMapper>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns((Business)null);

            // Act
            var imagesController = new ImagesController(mockBusinessService.Object, mockMenuItemService.Object, mockImageService.Object, mockMapper.Object);
            var actualResult = imagesController.GetAllByBusinessId(business.BusinessId);
           
            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }

        [Fact]
        public void GetAllByMenuItemId_MenuItemId_ExpectOk()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = 1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockImageService = new Mock<IImageService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns(menuItem);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<Image>, IEnumerable<ImageApiModel>>(It.IsAny<IEnumerable<Image>>()))
                      .Returns(new List<ImageApiModel>() { new ImageApiModel { Id = 1 } });

            // Act
            var imagesController = new ImagesController(mockBusinessService.Object, mockMenuItemService.Object, mockImageService.Object, mockMapper.Object);
            var actualResult = imagesController.GetAllByMenuItemId(menuItem.MenuItemId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<IEnumerable<Image>, IEnumerable<ImageApiModel>>(It.IsAny<IEnumerable<Image>>()), Times.Once);
        }

        [Fact]
        public void GetAllByMenuItemId_MenuItemIdNotFound_ExpectNotFound()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = -1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockImageService = new Mock<IImageService>();
            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns((MenuItem)null);

            // Act
            var imagesController = new ImagesController(mockBusinessService.Object, mockMenuItemService.Object, mockImageService.Object, mockMapper.Object);
            var actualResult = imagesController.GetAllByBusinessId(menuItem.MenuItemId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }

        [Fact]
        public void Get_ImageId_ExpectOk()
        {
            // Arrange
            var image = new Image { ImageId = 1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.Get(It.IsAny<int>())).Returns(image);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Image, ImageApiModel>(It.IsAny<Image>()))
                      .Returns(new ImageApiModel { Id = 1 });

            // Act
            var imagesController = new ImagesController(mockBusinessService.Object, mockMenuItemService.Object, mockImageService.Object, mockMapper.Object);
            var actualResult = imagesController.Get(image.ImageId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<Image, ImageApiModel>(It.IsAny<Image>()), Times.Once);
        }

        [Fact]
        public void Get_ImageIdNotFound_ExpectNotFound()
        {
            // Arrange
            var image = new Image { ImageId = -1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockMapper = new Mock<IMapper>();
            var mockImageService = new Mock<IImageService>();
            mockImageService.Setup(x => x.Get(It.IsAny<int>())).Returns((Image)null);

            // Act
            var imagesController = new ImagesController(mockBusinessService.Object, mockMenuItemService.Object, mockImageService.Object, mockMapper.Object);
            var actualResult = imagesController.Get(image.ImageId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
    }
}
