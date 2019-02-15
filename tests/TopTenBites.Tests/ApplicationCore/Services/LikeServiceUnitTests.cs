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
    public class LikeServiceUnitTests
    {
        [Fact]
        public void Get_IdFound_ExpectObject()
        {
            // Arrange
            var like = new Like { LikeId = 1 };

            var mockRepository = new Mock<IGenericRepository<Like>>();
            mockRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(like);

            // Act
            var likeService = new LikeService(mockRepository.Object);
            var actualLike = likeService.Get(like.LikeId);

            // Assert
            mockRepository.Verify(x => x.GetById(It.IsAny<int>()));
            Assert.Equal(like.LikeId, actualLike.LikeId);
        }

        [Fact]
        public void GetAll_IdsFilterEmpty_ExpectObjects()
        {
            // Arrange
            IList<Like> likes = new List<Like>
            {
                new Like() { LikeId = 1 },
                new Like() { LikeId = 2 },
                new Like() { LikeId = 3 }
            };
            var likeIds = string.Empty;

            var mockRepository = new Mock<IGenericRepository<Like>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Like, bool>>>(),
                It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(),
                It.IsAny<string>()))
                         .Returns(likes);

            // Act
            var likeService = new LikeService(mockRepository.Object);
            var actualLikes = likeService.GetAll(likeIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Like, bool>>>(),
                It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(),
                It.IsAny<string>()));
            Assert.Equal(likes.Count, actualLikes.Count());
        }

        [Fact]
        public void GetAll_IdsFound_ExpectObjects()
        {
            // Arrange
            IList<Like> likes = new List<Like>
            {
                new Like() { LikeId = 1 },
                new Like() { LikeId = 2 },
                new Like() { LikeId = 3 }
            };
            var likeIds = "1, 2";
            var expectedLikes = likes.Where(x => likeIds.Split(',').Select(int.Parse).ToArray().Contains(x.LikeId));

            var mockRepository = new Mock<IGenericRepository<Like>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Like, bool>>>(),
                It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(), 
                It.IsAny<string>()))
                         .Returns(expectedLikes);

            // Act
            var likeService = new LikeService(mockRepository.Object);
            var actualLikes = likeService.GetAll(likeIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Like, bool>>>(),
                It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(),
                It.IsAny<string>()));

            Assert.Equal(expectedLikes.Count(), actualLikes.Count());
        }

        [Fact]
        public void GetAll_IdsNotFound_ExpectNull()
        {
            // Arrange
            IList<Like> likes = new List<Like>
            {
                new Like() { LikeId = 1 },
                new Like() { LikeId = 2 },
                new Like() { LikeId = 3 }
            };
            var likeIds = "-1";
            var expectedLikes = likes.Where(x => likeIds.Split(',').Select(int.Parse).ToArray().Contains(x.LikeId));

            var mockRepository = new Mock<IGenericRepository<Like>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Like, bool>>>(),
                It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(),
                It.IsAny<string>()))
                         .Returns(expectedLikes);

            // Act
            var likeService = new LikeService(mockRepository.Object);
            var actualLikes = likeService.GetAll(likeIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Like, bool>>>(),
                It.IsAny<Func<IQueryable<Like>, IOrderedQueryable<Like>>>(),
                It.IsAny<string>()));

            Assert.Empty(actualLikes);
            Assert.Equal(expectedLikes.Count(), actualLikes.Count());
        }
    }
}
