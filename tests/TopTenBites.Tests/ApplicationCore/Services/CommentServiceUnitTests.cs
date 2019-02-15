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
    public class CommentServiceUnitTests
    {
        [Fact]
        public void Get_IdFound_ExpectObject()
        {
            // Arrange
            var comment = new Comment { CommentId = 1 };

            var mockRepository = new Mock<IGenericRepository<Comment>>();
            mockRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(comment);

            // Act
            var commentService = new CommentService(mockRepository.Object);
            var actualComment = commentService.Get((int)comment.CommentId);

            // Assert
            mockRepository.Verify(x => x.GetById(It.IsAny<int>()));
            Assert.Equal(comment.CommentId, actualComment.CommentId);
        }

        [Fact]
        public void GetAll_IdsFilterEmpty_ExpectObjects()
        {
            // Arrange
            IList<Comment> comments = new List<Comment>
            {
                new Comment() { CommentId = 1 },
                new Comment() { CommentId = 2 },
                new Comment() { CommentId = 3 }
            };
            var commentIds = string.Empty;

            var mockRepository = new Mock<IGenericRepository<Comment>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                It.IsAny<string>()))
                         .Returns(comments);

            // Act
            var commentService = new CommentService(mockRepository.Object);
            var actualComments = commentService.GetAll(commentIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                It.IsAny<string>()));
            Assert.Equal(comments.Count, actualComments.Count());
        }

        [Fact]
        public void GetAll_IdsFound_ExpectObjects()
        {
            // Arrange
            IList<Comment> comments = new List<Comment>
            {
                new Comment() { CommentId = 1 },
                new Comment() { CommentId = 2 },
                new Comment() { CommentId = 3 }
            };
            var commentIds = "1, 2";
            var expectedComments = comments.Where(x => commentIds.Split(',').Select(int.Parse).ToArray().Contains((int)x.CommentId));

            var mockRepository = new Mock<IGenericRepository<Comment>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(), 
                It.IsAny<string>()))
                         .Returns(expectedComments);

            // Act
            var commentService = new CommentService(mockRepository.Object);
            var actualComments = commentService.GetAll(commentIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                It.IsAny<string>()));

            Assert.Equal(expectedComments.Count(), actualComments.Count());
        }

        [Fact]
        public void GetAll_IdsNotFound_ExpectNull()
        {
            // Arrange
            IList<Comment> comments = new List<Comment>
            {
                new Comment() { CommentId = 1 },
                new Comment() { CommentId = 2 },
                new Comment() { CommentId = 3 }
            };
            var commentIds = "-1";
            var expectedComments = comments.Where(x => commentIds.Split(',').Select(int.Parse).ToArray().Contains((int)x.CommentId));

            var mockRepository = new Mock<IGenericRepository<Comment>>();
            mockRepository.Setup(x => x.GetAll(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                It.IsAny<string>()))
                         .Returns(expectedComments);

            // Act
            var commentService = new CommentService(mockRepository.Object);
            var actualComments = commentService.GetAll(commentIds);

            // Assert
            mockRepository.Verify(x => x.GetAll(
                It.IsAny<Expression<Func<Comment, bool>>>(),
                It.IsAny<Func<IQueryable<Comment>, IOrderedQueryable<Comment>>>(),
                It.IsAny<string>()));

            Assert.Empty(actualComments);
            Assert.Equal(expectedComments.Count(), actualComments.Count());
        }

        [Fact]
        public void Add_Entity_ExpectSuccess()
        {
            // Arrange
            var newComment = new Comment() { CommentId = 1, Text = "Text1" };

            var mockRepository = new Mock<IGenericRepository<Comment>>();
            mockRepository.Setup(x => x.Add(It.IsAny<Comment>())).Returns(newComment);

            // Act
            var commentService = new CommentService(mockRepository.Object);
            var actualComment = commentService.Add(newComment);

            // Assert
            mockRepository.Verify(x => x.Add(It.IsAny<Comment>()));
            Assert.Equal(newComment.Text, actualComment.Text);
        }

        [Fact]
        public void Update_Entity_ExpectSuccess()
        {
            // Arrange
            var newComment = new Comment() { CommentId = 1, Text = "Text1" };

            var mockRepository = new Mock<IGenericRepository<Comment>>();
            mockRepository.Setup(x => x.Update(It.IsAny<Comment>()));

            // Act
            var commentService = new CommentService(mockRepository.Object);
            commentService.Update(newComment);

            // Assert
            mockRepository.Verify(x => x.Update(It.IsAny<Comment>()));
        }
    }
}
