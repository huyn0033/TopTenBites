using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using TopTenBites.Web.Api.v1.ApiModels;
using TopTenBites.Web.Api.v1.Controllers;
using TopTenBites.Web.ApplicationCore.Enums;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;
using Xunit;

namespace TopTenBites.Tests.Api.v1
{
    public class CommentsControllerUnitTests
    {
        #region GetAllByBusinessId
        [Fact]
        public void GetAllByBusinessId_BusinessId_ExpectOk()
        {
            // Arrange
            var business = new Business { BusinessId = 1 };

            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns(business);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<Comment>, IEnumerable<CommentApiModel>>(It.IsAny<IEnumerable<Comment>>()))
                      .Returns(new List<CommentApiModel>() { new CommentApiModel { Id = 1 } });

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.GetAllByBusinessId(business.BusinessId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<IEnumerable<Comment>, IEnumerable<CommentApiModel>>(It.IsAny<IEnumerable<Comment>>()), Times.Once);
        }

        [Fact]
        public void GetAllByBusinessId_BusinessIdNotFound_ExpectNotFound()
        {
            // Arrange
            var business = new Business { BusinessId = -1 };

            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();
            mockBusinessService.Setup(x => x.GetByBusinessId(It.IsAny<int>())).Returns((Business)null);
            var mockMapper = new Mock<IMapper>();
            
            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.GetAllByBusinessId(business.BusinessId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
        #endregion

        #region GetAllByMenuItemId
        [Fact]
        public void GetAllByMenuItemId_MenuItemId_ExpectOk()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = 1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns(menuItem);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<IEnumerable<Comment>, IEnumerable<CommentApiModel>>(It.IsAny<IEnumerable<Comment>>()))
                      .Returns(new List<CommentApiModel>() { new CommentApiModel { Id = 1 } });

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.GetAllByMenuItemId(menuItem.MenuItemId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<IEnumerable<Comment>, IEnumerable<CommentApiModel>>(It.IsAny<IEnumerable<Comment>>()), Times.Once);
        }

        [Fact]
        public void GetAllByMenuItemId_MenuItemIdNotFound_ExpectNotFound()
        {
            // Arrange
            var menuItem = new MenuItem { MenuItemId = -1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            mockMenuItemService.Setup(x => x.Get(It.IsAny<int>())).Returns((MenuItem)null);

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.GetAllByBusinessId(menuItem.MenuItemId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
        #endregion

        #region Get
        [Fact]
        public void Get_CommentId_ExpectOk()
        {
            // Arrange
            var comment = new Comment { CommentId = 1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            mockCommentService.Setup(x => x.Get(It.IsAny<int>())).Returns(comment);
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(x => x.Map<Comment, CommentApiModel>(It.IsAny<Comment>()))
                      .Returns(new CommentApiModel { Id = 1 });

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Get((int)comment.CommentId);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<Comment, CommentApiModel>(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public void Get_CommentIdNotFound_ExpectNotFound()
        {
            // Arrange
            var comment = new Comment { CommentId = -1 };

            var mockBusinessService = new Mock<IBusinessService>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockMapper = new Mock<IMapper>();
            var mockCommentService = new Mock<ICommentService>();
            mockCommentService.Setup(x => x.Get(It.IsAny<int>())).Returns((Comment)null);

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Get((int)comment.CommentId);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
        #endregion

        #region Post
        [Fact]
        public void Post_CommentApiModel_ExpectOk()
        {
            // Arrange
            var commentVM = new CommentApiModel() { /*Id = 2,*/ MenuItemId = 1, Text = "Text1", Sentiment = Sentiment.Positive };
            var comment = new Comment() { CommentId = 2, MenuItemId = 1, Text = "Text1", Sentiment = Sentiment.Positive };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            mockCommentService.Setup(x => x.Add(It.IsAny<Comment>())).Returns(comment);
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Post(commentVM);

            // Assert
            Assert.IsType<OkObjectResult>(actualResult);
            mockMapper.Verify(x => x.Map<CommentApiModel, Comment>(It.IsAny<CommentApiModel>()), Times.Once);
            mockMapper.Verify(x => x.Map<Comment, CommentApiModel>(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public void Post_CommentApiModelIdNotEmpty_ExpectBadRequest()
        {
            // Arrange
            var commentVM = new CommentApiModel() { Id = 2, MenuItemId = 1, Text = "Text1", Sentiment = Sentiment.Positive };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Post(commentVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_MenuItemApiModelTextLongerThan140_ExpectBadRequest()
        {
            // Arrange
            var commentVM = new CommentApiModel() { /*Id = 2,*/
                MenuItemId = 1,
                Sentiment = Sentiment.Positive,
                Text = new string('a', 141)
            };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Post(commentVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_CommentApiModelSentimentEmpty_ExpectBadRequest()
        {
            // Arrange
            var commentVM = new CommentApiModel() { Id = 2, MenuItemId = 1, Text = "Text1" /*,Sentiment = Sentiment.Positive*/ };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Post(commentVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Post_CommentApiModelMenuItemIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var commentVM = new CommentApiModel() { Id = 2, /*MenuItemId = 1,*/ Text = "Text1" ,Sentiment = Sentiment.Positive };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Post(commentVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }
        #endregion

        #region Put
        [Fact]
        public void Put_CommentApiModel_ExpectOk()
        {
            // Arrange
            var commentVM = new CommentApiModel() { Id = 2, MenuItemId = 1, Text = "Text1", Sentiment = Sentiment.Positive };
            var comment = new Comment() { MenuItemId = 1, Text = "Text2", Sentiment = Sentiment.Positive };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            mockCommentService.Setup(x => x.Get(It.IsAny<int>())).Returns(comment);
            mockCommentService.Setup(x => x.Update(It.IsAny<Comment>()));
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Put(commentVM);

            // Assert
            Assert.IsType<OkResult>(actualResult);
            mockMapper.Verify(x => x.Map<CommentApiModel, Comment>(It.IsAny<CommentApiModel>(), It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public void Put_CommentApiModelIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var commentVM = new CommentApiModel() { /*Id = 2,*/ MenuItemId = 1, Text = "Text1", Sentiment = Sentiment.Positive };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Put(commentVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }
    
        [Fact]
        public void Put_MenuItemApiModelTextLongerThan140_ExpectBadRequest()
        {
            // Arrange
            var commentVM = new CommentApiModel()
            { /*Id = 2,*/
                MenuItemId = 1,
                Sentiment = Sentiment.Positive,
                Text = new string('a', 141)
            };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Put(commentVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_MenuItemApiModelSentimentEmpty_ExpectBadRequest()
        {
            // Arrange
            var commentVM = new CommentApiModel() { /*Id = 2,*/ MenuItemId = 1, Text = "Text1"/*, Sentiment = Sentiment.Positive*/ };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Put(commentVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_CommentApiModelMenuItemIdEmpty_ExpectBadRequest()
        {
            // Arrange
            var commentVM = new CommentApiModel() { Id = 2, /*MenuItemId = 1,*/ Text = "Text1", Sentiment = Sentiment.Positive };

            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Put(commentVM);

            // Assert
            Assert.IsType<BadRequestObjectResult>(actualResult);
        }

        [Fact]
        public void Put_CommentApiModelIdNotFound_ExpectNotFound()
        {
            // Arrange
            var commentVM = new CommentApiModel() { Id = -1, MenuItemId = 1, Text = "Text1", Sentiment = Sentiment.Positive };
            
            var mockMapper = new Mock<IMapper>();
            var mockMenuItemService = new Mock<IMenuItemService>();
            var mockCommentService = new Mock<ICommentService>();
            mockCommentService.Setup(x => x.Get(It.IsAny<int>())).Returns((Comment)null);
            var mockBusinessService = new Mock<IBusinessService>();

            // Act
            var commentsController = new CommentsController(mockBusinessService.Object, mockMenuItemService.Object, mockCommentService.Object, mockMapper.Object);
            var actualResult = commentsController.Put(commentVM);

            // Assert
            Assert.IsType<NotFoundResult>(actualResult);
        }
        #endregion
    }
}
