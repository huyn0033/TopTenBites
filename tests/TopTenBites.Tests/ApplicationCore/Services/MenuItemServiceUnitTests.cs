using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;
using TopTenBites.Web.ApplicationCore.Services;
using Xunit;

namespace TopTenBites.Tests.ApplicationCore.Services
{
    public class MenuItemServiceUnitTests
    {
        [Fact]
        public void Get_IdFound_ExpectObject()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = 1 };

            var mockRepository = new Mock<IGenericRepository<MenuItem>>();
            mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<MenuItem, bool>>>(), It.IsAny<string>())).Returns(menuItem);

            // Act
            var menuItemService = new MenuItemService(mockRepository.Object);
            var actualMenuItem = menuItemService.Get((int)menuItem.MenuItemId);

            // Assert
            mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<MenuItem, bool>>>(), It.IsAny<string>()));
            Assert.Equal(menuItem.MenuItemId, actualMenuItem.MenuItemId);
        }

        [Fact]
        public void GetAll_IdsFilterEmpty_ExpectObjects()
        {
            // Arrange
            IList<MenuItem> menuItems = new List<MenuItem>
            {
                new MenuItem() { MenuItemId = 1 },
                new MenuItem() { MenuItemId = 2 },
                new MenuItem() { MenuItemId = 3 }
            };
            var menuItemIds = string.Empty;

            var mockRepository = new Mock<IGenericRepository<MenuItem>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<MenuItem, bool>>>(),
                It.IsAny<Func<IQueryable<MenuItem>, IOrderedQueryable<MenuItem>>>(),
                It.IsAny<string>()))
                         .Returns(menuItems);

            // Act
            var menuItemService = new MenuItemService(mockRepository.Object);
            var actualMenuItems = menuItemService.GetAll(menuItemIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<MenuItem, bool>>>(),
                It.IsAny<Func<IQueryable<MenuItem>, IOrderedQueryable<MenuItem>>>(),
                It.IsAny<string>()));
            Assert.Equal(menuItems.Count, actualMenuItems.Count());
        }

        [Fact]
        public void GetAll_IdsFound_ExpectObjects()
        {
            // Arrange
            IList<MenuItem> menuItems = new List<MenuItem>
            {
                new MenuItem() { MenuItemId = 1 },
                new MenuItem() { MenuItemId = 2 },
                new MenuItem() { MenuItemId = 3 }
            };
            var menuItemIds = "1, 2";
            var expectedMenuItems = menuItems.Where(x => menuItemIds.Split(',').Select(int.Parse).ToArray().Contains((int)x.MenuItemId));

            var mockRepository = new Mock<IGenericRepository<MenuItem>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<MenuItem, bool>>>(),
                It.IsAny<Func<IQueryable<MenuItem>, IOrderedQueryable<MenuItem>>>(), 
                It.IsAny<string>()))
                         .Returns(expectedMenuItems);

            // Act
            var menuItemService = new MenuItemService(mockRepository.Object);
            var actualMenuItems = menuItemService.GetAll(menuItemIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<MenuItem, bool>>>(),
                It.IsAny<Func<IQueryable<MenuItem>, IOrderedQueryable<MenuItem>>>(),
                It.IsAny<string>()));

            Assert.Equal(expectedMenuItems.Count(), actualMenuItems.Count());
        }

        [Fact]
        public void GetAll_IdsNotFound_ExpectNull()
        {
            // Arrange
            IList<MenuItem> menuItems = new List<MenuItem>
            {
                new MenuItem() { MenuItemId = 1 },
                new MenuItem() { MenuItemId = 2 },
                new MenuItem() { MenuItemId = 3 }
            };
            var menuItemIds = "-1";
            var expectedMenuItems = menuItems.Where(x => menuItemIds.Split(',').Select(int.Parse).ToArray().Contains((int)x.MenuItemId));

            var mockRepository = new Mock<IGenericRepository<MenuItem>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<MenuItem, bool>>>(),
                It.IsAny<Func<IQueryable<MenuItem>, IOrderedQueryable<MenuItem>>>(),
                It.IsAny<string>()))
                         .Returns(expectedMenuItems);

            // Act
            var menuItemService = new MenuItemService(mockRepository.Object);
            var actualMenuItems = menuItemService.GetAll(menuItemIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<MenuItem, bool>>>(),
                It.IsAny<Func<IQueryable<MenuItem>, IOrderedQueryable<MenuItem>>>(),
                It.IsAny<string>()));

            Assert.Empty(actualMenuItems);
            Assert.Equal(expectedMenuItems.Count(), actualMenuItems.Count());
        }

        [Fact]
        public void Add_Entity_ExpectSuccess()
        {
            // Arrange
            var newMenuItem = new MenuItem() { MenuItemId = 1, Name = "Name1" };

            var mockRepository = new Mock<IGenericRepository<MenuItem>>();
            mockRepository.Setup(x => x.Add(It.IsAny<MenuItem>())).Returns(newMenuItem);

            // Act
            var menuItemService = new MenuItemService(mockRepository.Object);
            var actualMenuItem = menuItemService.Add(newMenuItem);

            // Assert
            mockRepository.Verify(x => x.Add(It.IsAny<MenuItem>()));
            Assert.Equal(newMenuItem.Name, actualMenuItem.Name);
        }

        [Fact]
        public void Update_Entity_ExpectSuccess()
        {
            // Arrange
            var newMenuItem = new MenuItem() { MenuItemId = 1, Name = "Name1" };

            var mockRepository = new Mock<IGenericRepository<MenuItem>>();
            mockRepository.Setup(x => x.Update(It.IsAny<MenuItem>()));

            // Act
            var menuItemService = new MenuItemService(mockRepository.Object);
            menuItemService.Update(newMenuItem);

            // Assert
            mockRepository.Verify(x => x.Update(It.IsAny<MenuItem>()));
        }
    }
}
