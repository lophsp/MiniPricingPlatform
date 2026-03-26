using Microsoft.AspNetCore.Mvc;
using Moq;
using MiniPricingPlatform.API.Controllers;
using MiniPricingPlatform.API.DTOs;
using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Domain.Rules;
using MiniPricingPlatform.Domain.Entities;

namespace MiniPricingPlatform.Tests.Controllers
{
    public class RuleControllerTests
    {
        private readonly Mock<IRuleService> _ruleServiceMock;
        private readonly RuleController _controller;

        public RuleControllerTests()
        {
            _ruleServiceMock = new Mock<IRuleService>();
            _controller = new RuleController(_ruleServiceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithRules()
        {
            // Arrange
            var rules = new List<PricingRule>
            {
                new WeightTierRule { Id = Guid.NewGuid(), Priority = 1, IsActive = true }
            };
            _ruleServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(rules);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRules = Assert.IsAssignableFrom<IEnumerable<RuleResponseDto>>(okResult.Value);
            Assert.Single(returnedRules);
            Assert.Equal("weighttier", returnedRules.First().Type);
        }

        [Fact]
        public async Task GetById_RuleExists_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var rule = new RemoteAreaRule { Id = id, Priority = 2, IsActive = true };
            _ruleServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(rule);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<RuleResponseDto>(okResult.Value);
            Assert.Equal("remotearea", returned.Type);
        }

        [Fact]
        public async Task GetById_RuleNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _ruleServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((PricingRule)null);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreated()
        {
            // Arrange
            var dto = new RuleRequestDto
            {
                Type = "weighttier",
                Priority = 1,
                IsActive = true,
                Price = 100,
                Min = 1,
                Max = 10
            };
            var createdRule = new WeightTierRule
            {
                Id = Guid.NewGuid(),
                Priority = dto.Priority,
                IsActive = dto.IsActive,
                Price = dto.Price.Value,
                Min = dto.Min.Value,
                Max = dto.Max.Value
            };
            _ruleServiceMock.Setup(s => s.CreateAsync(It.IsAny<PricingRule>())).ReturnsAsync(createdRule);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returned = Assert.IsType<RuleResponseDto>(createdResult.Value);
            Assert.Equal("weighttier", returned.Type);
        }

        [Fact]
        public async Task Update_RuleExists_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var existing = new RemoteAreaRule { Id = id };
            var dto = new RuleRequestDto
            {
                Type = "remoteareasurcharge",
                Priority = 2,
                IsActive = true,
                RemoteSurcharge = 50
            };

            _ruleServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(existing);
            _ruleServiceMock.Setup(s => s.UpdateAsync(It.IsAny<PricingRule>())).ReturnsAsync(
                new RemoteAreaRule { Id = id, Priority = dto.Priority, Surcharge = dto.RemoteSurcharge.Value, IsActive = dto.IsActive });

            // Act
            var result = await _controller.Update(id, dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returned = Assert.IsType<RuleResponseDto>(okResult.Value);
            Assert.Equal("remotearea", returned.Type);
            Assert.Equal(dto.Priority, returned.Priority);
        }

        [Fact]
        public async Task Update_RuleNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var dto = new RuleRequestDto { Type = "weighttier" };
            _ruleServiceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((PricingRule)null);

            // Act
            var result = await _controller.Update(id, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _ruleServiceMock.Verify(s => s.DeleteAsync(id), Times.Once);
        }
    }
}