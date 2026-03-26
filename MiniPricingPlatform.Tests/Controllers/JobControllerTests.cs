using Microsoft.AspNetCore.Mvc;
using MiniPricingPlatform.API.Controllers;
using MiniPricingPlatform.API.DTOs;
using MiniPricingPlatform.Application.Models;
using Moq;

namespace MiniPricingPlatform.API.Tests.Controllers
{
    public class JobControllerTests
    {
        private readonly Mock<IJobService> _mockJobService;
        private readonly JobsController _controller;

        public JobControllerTests()
        {
            _mockJobService = new Mock<IJobService>();
            _controller = new JobsController(_mockJobService.Object);
        }

        [Fact]
        public void Get_ShouldReturnJobDetail_WhenJobExists()
        {
            // Arrange
            var job = new JobDetailResponse
            {
                Status = "completed",
                Results = new List<JobResultItem>
                {
                    new JobResultItem { Weight = 5, Area = "city", Price = 100 },
                    new JobResultItem { Weight = 5, Area = "remote", Price = 150 },
                    new JobResultItem { Weight = 5, Area = "remote", Price = 135 },
                    new JobResultItem { Weight = 15, Area = "city", Price = 150 },
                }
            };

            _mockJobService.Setup(s => s.Get("job-123")).Returns(job);

            // Act
            var result = _controller.Get("job-123");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<JobDetailResponseDto>(okResult.Value);
            Assert.Equal("completed", data.Status);
            Assert.Equal(4, data.Results.Count);
            Assert.Equal(100, data.Results[0].Price);
            Assert.Equal("city", data.Results[0].Area);
        }

        [Fact]
        public void Get_ShouldReturn404_WhenJobNotExists()
        {
            // Arrange
            _mockJobService.Setup(s => s.Get("job-123")).Returns((JobDetailResponse?)null);

            // Act
            var result = _controller.Get("job-123");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}