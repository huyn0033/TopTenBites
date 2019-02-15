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
    public class ImageServiceUnitTests
    {
        [Fact]
        public void Get_IdFound_ExpectObject()
        {
            // Arrange
            var image = new Image { ImageId = 1 };

            var mockRepository = new Mock<IGenericRepository<Image>>();
            mockRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(image);

            // Act
            var imageService = new ImageService(mockRepository.Object);
            var actualImage = imageService.Get(image.ImageId);

            // Assert
            mockRepository.Verify(x => x.GetById(It.IsAny<int>()));
            Assert.Equal(image.ImageId, actualImage.ImageId);
        }

        [Fact]
        public void GetAll_IdsFilterEmpty_ExpectObjects()
        {
            // Arrange
            IList<Image> images = new List<Image>
            {
                new Image() { ImageId = 1 },
                new Image() { ImageId = 2 },
                new Image() { ImageId = 3 }
            };
            var ImageIds = string.Empty;

            var mockRepository = new Mock<IGenericRepository<Image>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IOrderedQueryable<Image>>>(),
                It.IsAny<string>()))
                         .Returns(images);

            // Act
            var imageService = new ImageService(mockRepository.Object);
            var actualImages = imageService.GetAll(ImageIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IOrderedQueryable<Image>>>(),
                It.IsAny<string>()));
            Assert.Equal(images.Count, actualImages.Count());
        }

        [Fact]
        public void GetAll_IdsFound_ExpectObjects()
        {
            // Arrange
            IList<Image> images = new List<Image>
            {
                new Image() { ImageId = 1 },
                new Image() { ImageId = 2 },
                new Image() { ImageId = 3 }
            };
            var ImageIds = "1, 2";
            var expectedImages = images.Where(x => ImageIds.Split(',').Select(int.Parse).ToArray().Contains(x.ImageId));

            var mockRepository = new Mock<IGenericRepository<Image>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IOrderedQueryable<Image>>>(), 
                It.IsAny<string>()))
                         .Returns(expectedImages);

            // Act
            var imageService = new ImageService(mockRepository.Object);
            var actualImages = imageService.GetAll(ImageIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IOrderedQueryable<Image>>>(),
                It.IsAny<string>()));

            Assert.Equal(expectedImages.Count(), actualImages.Count());
        }

        [Fact]
        public void GetAll_IdsNotFound_ExpectNull()
        {
            // Arrange
            IList<Image> images = new List<Image>
            {
                new Image() { ImageId = 1 },
                new Image() { ImageId = 2 },
                new Image() { ImageId = 3 }
            };
            var ImageIds = "-1";
            var expectedImages = images.Where(x => ImageIds.Split(',').Select(int.Parse).ToArray().Contains(x.ImageId));

            var mockRepository = new Mock<IGenericRepository<Image>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IOrderedQueryable<Image>>>(),
                It.IsAny<string>()))
                         .Returns(expectedImages);

            // Act
            var imageService = new ImageService(mockRepository.Object);
            var actualImages = imageService.GetAll(ImageIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Image, bool>>>(),
                It.IsAny<Func<IQueryable<Image>, IOrderedQueryable<Image>>>(),
                It.IsAny<string>()));

            Assert.Empty(actualImages);
            Assert.Equal(expectedImages.Count(), actualImages.Count());
        }
    }
}
