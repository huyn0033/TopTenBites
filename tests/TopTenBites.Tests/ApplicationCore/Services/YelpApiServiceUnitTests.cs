using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.Core.Services;
using Xunit;

namespace TopTenBites.Tests.ApplicationCore.Services
{
    public class YelpApiServiceUnitTests
    {
        [Theory]
        [InlineData("text1", "Current Location", "lat1", "lng1", 
            "lat=lat1&lng=lng1&is_new_loc=true&prefix=text1&is_initial_prefetch=")]
        [InlineData("text1", "", "lat1", "lng1",
            "loc=&loc_name_param=loc&is_new_loc=true&prefix=text1&is_initial_prefetch=")]
        public void GetYelpAutocompleteDescriptionQuery_WhenCalled_ReturnString(string text, string location, string lat, string lng, string expected)
        {
            // Arrange

            // Act
            var actual = YelpApiService.GetYelpAutocompleteDescriptionQuery(text, location, lat, lng);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("text1", "Current Location", "lat1", "lng1",
            "term=text1&latitude=lat1&longitude=lng1")]
        [InlineData("text1", "", "lat1", "lng1",
            "term=text1&location=")]
        public void GetYelpBusinessSearchQuery_WhenCalled_ReturnString(string text, string location, string lat, string lng, string expected)
        {
            // Arrange

            // Act
            var actual = YelpApiService.GetYelpBusinessSearchQuery(text, location, lat, lng);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("text1", "Current Location", "lat1", "lng1",
            "https://www.yelp.com/search_suggest/v2/prefetch?lat=lat1&lng=lng1&is_new_loc=true&prefix=text1&is_initial_prefetch=")]
        [InlineData("text1", "", "lat1", "lng1",
            "https://www.yelp.com/search_suggest/v2/prefetch?loc=&loc_name_param=loc&is_new_loc=true&prefix=text1&is_initial_prefetch=")]
        public async void GetYelpAutocompleteDescriptionAsync_WhenCalled_ExpectSuccess(string text, string location, string lat, string lng, string expected)
        {
            // Arrange
            var expectedUri = new Uri(expected);

            var mockAppSettingsService = new Mock<IAppSettingsService>();
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("test"),
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.yelp.com")
            };
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var yelpApiService = new YelpApiService(mockHttpClientFactory.Object, mockAppSettingsService.Object);
            var result = await yelpApiService.GetYelpAutocompleteDescriptionAsync(text, location, lat, lng);

            // Assert
            Assert.NotNull(result);
            mockHttpMessageHandler.Protected().Verify(
               "SendAsync",
               Times.Once(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async void GetYelpAutocompleteLocationAsync_WhenCalled_ExpectSuccess()
        {
            // Arrange
            var text = "text1";
            var expectedUri = new Uri($"https://www.yelp.com/location_suggest/v2?prefix={text}");

            var mockAppSettingsService = new Mock<IAppSettingsService>();
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("test"),
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://www.yelp.com")
            };
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var yelpApiService = new YelpApiService(mockHttpClientFactory.Object, mockAppSettingsService.Object);
            var result = await yelpApiService.GetYelpAutocompleteLocationAsync(text);

            // Assert
            Assert.NotNull(result);
            mockHttpMessageHandler.Protected().Verify(
               "SendAsync",
               Times.Once(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }

        [Theory]
        [InlineData("text1", "Current Location", "lat1", "lng1",
            "https://api.yelp.com/v3/businesses/search?term=text1&latitude=lat1&longitude=lng1")]
        [InlineData("text1", "", "lat1", "lng1",
            "https://api.yelp.com/v3/businesses/search?term=text1&location=")]
        public async void GetYelpBusinessSearchAsync_WhenCalled_ExpectSuccess(string text, string location, string lat, string lng, string expected)
        {
            // Arrange
            var expectedUri = new Uri(expected);

            var mockAppSettingsService = new Mock<IAppSettingsService>();
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("test"),
                })
                .Verifiable();

            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://api.yelp.com/v3")
            };
            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var yelpApiService = new YelpApiService(mockHttpClientFactory.Object, mockAppSettingsService.Object);
            var result = await yelpApiService.GetYelpBusinessSearchAsync(text, location, lat, lng);

            // Assert
            Assert.NotNull(result);
            mockHttpMessageHandler.Protected().Verify(
               "SendAsync",
               Times.Once(),
               ItExpr.Is<HttpRequestMessage>(req =>
                  req.Method == HttpMethod.Get
                  && req.RequestUri == expectedUri
               ),
               ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
