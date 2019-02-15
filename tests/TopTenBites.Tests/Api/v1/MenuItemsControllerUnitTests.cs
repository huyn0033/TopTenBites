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
    public class MenuItemsControllerUnitTests
    {
        #region GetAllByBusinessId
        [Fact]
        public void GetAllByBusinessId_BusinessId_ExpectOk()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };

            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<MenuItem>, IEnumerable<MenuItemApiModel>>(It.IsAny<IEnumerable<MenuItem>>()))
                      .Returns(new List<MenuItemApiModel>() { new MenuItemApiModel { Id = 1 } });

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.GetAllByBusinessId(business.BusinessId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<IEnumerable<MenuItem>, IEnumerable<MenuItemApiModel>>(It.IsAny<IEnumerable<MenuItem>>()), Times.Once);
        }

        [Fact]
        public void GetAllByBusinessId_BusinessIdNotFound_ExpectNotFound()
        {
            // Arrange
            var business = new Business { BusinessId = -1 };

            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns((Business)null);
            var mockMapper = new Mock<IMapper>();
            
            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.GetAllByBusinessId(business.BusinessId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
        #endregion

        #region Get
        [Fact]
        public void Get_MenuItemId_ExpectOk()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = 1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns(menuItem);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<MenuItem, MenuItemApiModel>(It.IsAny<MenuItem>()))
                      .Returns(new MenuItemApiModel { Id = 1 });

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Get(menuItem.MenuItemId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<MenuItem, MenuItemApiModel>(It.IsAny<MenuItem>()), Times.Once);
        }

        [Fact]
        public void Get_LikeIdNotFound_ExpectNotFound()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = -1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns((MenuItem)null);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Get(menuItem.MenuItemId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
        #endregion

        #region Post
        [Fact]
        public void Post_MenuItemApiModel_ExpectOk()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            business.MenuItems.Add(new MenuItem() { /*MenuItemId = 1,*/ BusinessId = 1, Name = "Name1" });
            var menuItemVM = new MenuItemApiModel() { /*Id = 2,*/ BusinessId = 1, Name = "Name2" };
            var menuItem = new MenuItem() { MenuItemId = 2, BusinessId = 1, Name = "Name2" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Add(It.IsAny<MenuItem>())).Returns(menuItem);
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Post(menuItemVM);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<MenuItemApiModel, MenuItem>(It.IsAny<MenuItemApiModel>()), Times.Once);
            mockMapper.Verify(x => x.Map<MenuItem, MenuItemApiModel>(It.IsAny<MenuItem>()), Times.Once);
        }

        [Fact]
        public void Post_MenuItemApiModelIdNotEmpty_ExpectBadRequest()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            var menuItemVM = new MenuItemApiModel() { Id = 1, BusinessId = 1, Name = "Name1" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Post(menuItemVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_MenuItemApiModelBusinessIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            var menuItemVM = new MenuItemApiModel() { /*Id = 1, BusinessId = 1,*/ Name = "Name1" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Post(menuItemVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_MenuItemApiModelNameLongerThan32_ExpectBadRequest()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            var menuItemVM = new MenuItemApiModel() { /*Id = 1,*/
                BusinessId = 1,
                Name = new string('a', 33)
            };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Post(menuItemVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_MenuItemApiModelNameDuplicate_ExpectBadRequest()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            business.MenuItems.Add(new MenuItem() { /*MenuItemId = 1,*/ BusinessId = 1, Name = "Name1" });
            var menuItemVM = new MenuItemApiModel() { /*Id = 2,*/ BusinessId = 1, Name = "Name1" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Post(menuItemVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }
        #endregion

        #region Put
        [Fact]
        public void Put_MenuItemApiModel_ExpectOk()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            business.MenuItems.Add(new MenuItem() { /*MenuItemId = 1,*/ BusinessId = 1, Name = "Name1" });
            var menuItemVM = new MenuItemApiModel() { Id = 1, BusinessId = 1, Name = "Name2" };
            var menuItem = new MenuItem() { MenuItemId = 1, BusinessId = 1, Name = "Name2" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns(menuItem);
            mockMenuItemService.Setup(x => x.Update(It.IsAny<MenuItem>()));
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Put(menuItemVM);

            // Assert
            Assert.IsType<OkResult>(actualResult);
            mockMapper.Verify(x => x.Map<MenuItemApiModel, MenuItem>(It.IsAny<MenuItemApiModel>(), It.IsAny<MenuItem>()), Times.Once);
        }

        [Fact]
        public void Put_MenuItemApiModelIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            var menuItemVM = new MenuItemApiModel() { /*Id = 1,*/ BusinessId = 1, Name = "Name1" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Put(menuItemVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_MenuItemApiModelBusinessIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            var menuItemVM = new MenuItemApiModel() { /*Id = 1, BusinessId = 1,*/ Name = "Name1" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Put(menuItemVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_MenuItemApiModelNameLongerThan32_ExpectBadRequest()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            var menuItemVM = new MenuItemApiModel() { /*Id = 1,*/ BusinessId = 1, Name = "Name12345678901234567890123456789" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Put(menuItemVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_MenuItemApiModelNameDuplicate_ExpectBadRequest()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            business.MenuItems.Add(new MenuItem() { /*MenuItemId = 1,*/ BusinessId = 1, Name = "Name1" });
            var menuItemVM = new MenuItemApiModel() { /*Id = 2,*/ BusinessId = 1, Name = "Name1" };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Put(menuItemVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_MenuItemApiModelIdNotFound_ExpectNotFound()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };
            business.MenuItems.Add(new MenuItem() { /*MenuItemId = 1,*/ BusinessId = 1, Name = "Name1" });
            var menuItemVM = new MenuItemApiModel() { Id = -1, BusinessId = 1, Name = "Name2" };
            
            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns((MenuItem)null);
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);

            // Act
            var menuItemsController = new MenuItemsController(mockBusinessService.Object, mockMenuItemService.Object, mockMapper.Object);
            var actualResult = menuItemsController.Put(menuItemVM);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
        #endregion
    }
}
