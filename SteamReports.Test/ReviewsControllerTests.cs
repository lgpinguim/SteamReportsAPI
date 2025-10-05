using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using SteamReports.Application.Interfaces;
using SteamReports.Application.ViewModels;
using SteamReports.Domain.Core.Helpers.Pagination;
using SteamReports.Domain.Models;
using SteamReportsAPI.Controllers;
using System.Text;
using System.Text.Json;

namespace SteamReports.Test
{
    public class ReviewsControllerTests
    {
        private readonly Mock<IReviewAppService> _mockReviewService;
        private readonly Mock<IDistributedCache> _mockCache;
        private readonly ReviewsController _controller;

        public ReviewsControllerTests()
        {
            _mockReviewService = new Mock<IReviewAppService>();
            _mockCache = new Mock<IDistributedCache>();
            _controller = new ReviewsController(_mockReviewService.Object, _mockCache.Object);
        }

        [Fact]
        public void GetReviews_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                PageNumber = 1,
                PageSize = 10
            };

            var reviews = new List<Review>
            {
                new Review { Id = 1, ReviewText = "Great game!", SteamAppId = 730 },
                new Review { Id = 2, ReviewText = "Not bad", SteamAppId = 730 }
            };

            var pagedResponse = new PagedResponse<List<Review>>(reviews, 1, 10, 1, 2);

            _mockReviewService
                .Setup(s => s.GetReviews(searchParams))
                .Returns(pagedResponse);

            // Act
            var result = _controller.GetReviews(searchParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedResponse = Assert.IsType<PagedResponse<List<Review>>>(okResult.Value);

            Assert.Equal(2, returnedResponse.Data.Count);
            _mockReviewService.Verify(s => s.GetReviews(searchParams), Times.Once);
        }

        [Fact]
        public void GetReviews_WithEmptyResult_ReturnsNotFound()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                PageNumber = 1,
                PageSize = 10
            };

            var emptyResponse = new PagedResponse<List<Review>>(new List<Review>(), 1, 10, 0, 0);

            _mockReviewService
                .Setup(s => s.GetReviews(searchParams))
                .Returns(emptyResponse);

            // Act
            var result = _controller.GetReviews(searchParams);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<PagedResponse<List<Review>>>(notFoundResult.Value);
        }

        [Fact]
        public void GetReviews_WithInvalidDateFilter_ReturnsBadRequest()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                PageNumber = 1,
                PageSize = 10,
                StartDate = DateTime.UtcNow,
                EndDate = null // Only one date provided - invalid
            };

            // Act
            var result = _controller.GetReviews(searchParams);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid date filters", badRequestResult.Value);

            _mockReviewService.Verify(
                s => s.GetReviews(It.IsAny<SearchParametersViewModel>()),
                Times.Never
            );
        }

        [Fact]
        public void GetReviews_WithEndDateBeforeStartDate_ReturnsBadRequest()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                PageNumber = 1,
                PageSize = 10,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(-1) // End before start
            };

            // Act
            var result = _controller.GetReviews(searchParams);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid date filters", badRequestResult.Value);
        }

        [Fact]
        public void GetReviews_WithValidDateRange_ReturnsOkResult()
        {
            // Arrange
            var searchParams = new SearchParametersViewModel
            {
                PageNumber = 1,
                PageSize = 10,
                StartDate = DateTime.UtcNow.AddDays(-7),
                EndDate = DateTime.UtcNow
            };

            var reviews = new List<Review> { new Review { Id = 1, SteamAppId = 730 } };
            var pagedResponse = new PagedResponse<List<Review>>(reviews, 1, 10, 1, 1);

            _mockReviewService
                .Setup(s => s.GetReviews(searchParams))
                .Returns(pagedResponse);

            // Act
            var result = _controller.GetReviews(searchParams);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            _mockReviewService.Verify(s => s.GetReviews(searchParams), Times.Once);
        }

        [Fact]
        public void GetReviewsSummary_WithCachedData_ReturnsCachedResult()
        {
            // Arrange
            var expectedSummary = new List<GameReviewViewModel>
            {
                new GameReviewViewModel
                {
                    SteamAppId = 730,
                    GameName = "Dune: Awakening",
                    PositiveReviews = 1000,
                    NegativeReviews = 100,
                    TotalReviews = 1100
                }
            };

            var cachedJson = JsonSerializer.Serialize(expectedSummary);
            var cachedBytes = Encoding.UTF8.GetBytes(cachedJson);

            _mockCache
                .Setup(c => c.Get("summary"))
                .Returns(cachedBytes);

            // Act
            var result = _controller.GetReviewsSummary();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSummary = Assert.IsType<List<GameReviewViewModel>>(okResult.Value);

            Assert.Single(returnedSummary);
            Assert.Equal(730, returnedSummary[0].SteamAppId);
            Assert.Equal("Dune: Awakening", returnedSummary[0].GameName);

            // Verify service was NOT called
            _mockReviewService.Verify(
                s => s.GetSummary(),
                Times.Never
            );
        }

        [Fact]
        public void GetReviewsSummary_WithNoCachedData_FetchesFromServiceAndCaches()
        {
            // Arrange
            var expectedSummary = new List<GameReviewViewModel>
            {
                new GameReviewViewModel
                {
                    SteamAppId = 440,
                    GameName = "Conan Exiles",
                    PositiveReviews = 5000,
                    NegativeReviews = 500,
                    TotalReviews = 5500
                }
            };

            _mockCache
                .Setup(c => c.Get("summary"))
                .Returns((byte[])null!);

            _mockReviewService
                .Setup(s => s.GetSummary())
                .Returns(expectedSummary);

            // Act
            var result = _controller.GetReviewsSummary();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSummary = Assert.IsType<List<GameReviewViewModel>>(okResult.Value);

            Assert.Single(returnedSummary);
            Assert.Equal(440, returnedSummary[0].SteamAppId);

            // Verify service was called
            _mockReviewService.Verify(s => s.GetSummary(), Times.Once);

            // Verify cache was set with 10 minute expiration
            _mockCache.Verify(
                c => c.Set(
                    "summary",
                    It.IsAny<byte[]>(),
                    It.Is<DistributedCacheEntryOptions>(opt =>
                        opt.AbsoluteExpirationRelativeToNow == TimeSpan.FromMinutes(10)
                    )
                ),
                Times.Once
            );
        }

        [Fact]
        public void GetReviewsSummary_WithEmptyResult_ReturnsOkWithEmptyList()
        {
            // Arrange
            var emptySummary = new List<GameReviewViewModel>();

            _mockCache
                .Setup(c => c.Get("summary"))
                .Returns((byte[])null!);

            _mockReviewService
                .Setup(s => s.GetSummary())
                .Returns(emptySummary);

            // Act
            var result = _controller.GetReviewsSummary();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSummary = Assert.IsType<List<GameReviewViewModel>>(okResult.Value);
            Assert.Empty(returnedSummary);
        }
    }
}