using Microsoft.AspNetCore.Mvc;
using MiniPricingPlatform.API.Controllers;
using MiniPricingPlatform.API.DTOs;
using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Domain.Models;
using Moq;

namespace MiniPricingPlatform.API.Tests.Controllers
{
    public class QuotesControllerTests
    {
        private readonly Mock<IPricingService> _mockPricingService;
        private readonly Mock<IJobService> _mockJobService;
        private readonly QuotesController _controller;

        public QuotesControllerTests()
        {
            _mockPricingService = new Mock<IPricingService>();
            _mockJobService = new Mock<IJobService>();

            _controller = new QuotesController(_mockPricingService.Object, _mockJobService.Object);
        }

        [Theory]
        [InlineData(5, "city", 100)]       // weight tier 0-10
        [InlineData(15, "city", 150)]      // weight tier 10-20
        [InlineData(8, "remote", 150)]     // weight tier + remote surcharge
        [InlineData(5, "city", 90)]        // time window discount 10%
        public async Task Calculate_ShouldReturnCorrectPrice_BasedOnRules(int weight, string area, decimal expectedPrice)
        {
            // Arrange
            var req = new PricingRequestDto
            {
                Weight = weight,
                Area = area,
                RequestTime = DateTime.UtcNow
            };

            _mockPricingService
                .Setup(s => s.CalculateAsync(It.IsAny<PricingInput>()))
                .ReturnsAsync(expectedPrice);

            // Act
            var result = await _controller.Calculate(req);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<PricingResponseDto>(okResult.Value);
            Assert.Equal(expectedPrice, data.Price);
        }

        [Fact]
        public void Bulk_ShouldReturnJobId_ForJson()
        {
            // Arrange
            var items = new List<PricingRequestDto>
            {
                new() { Weight = 5, Area = "city", RequestTime = DateTime.UtcNow },
                new() { Weight = 15, Area = "remote", RequestTime = DateTime.UtcNow }
            };

            var bulkRequest = new BulkRequestDto
            {
                Type = "json",
                Items = items
            };

            _mockJobService
                .Setup(s => s.CreateJob("json", items))
                .Returns("job-123");

            // Act
            var result = _controller.Bulk(bulkRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<JobResponseDto>(okResult.Value);
            Assert.Equal("job-123", data.JobId);
        }

        [Fact]
        public void Bulk_ShouldReturnJobId_ForCsv()
        {
            // Arrange
            var csvText = "Weight,Area,RequestTime\n5,city,2026-03-25T10:00:00Z\n15,remote,2026-03-25T10:00:00Z";

            var bulkRequest = new BulkRequestDto
            {
                Type = "csv",
                Items = csvText
            };

            _mockJobService
                .Setup(s => s.CreateJob("csv", csvText))
                .Returns("job-456");

            // Act
            var result = _controller.Bulk(bulkRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsType<JobResponseDto>(okResult.Value);
            Assert.Equal("job-456", data.JobId);
        }
    }
}