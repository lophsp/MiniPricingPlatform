using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Application.Services;
using MiniPricingPlatform.Domain.Entities;
using Moq;

namespace MiniPricingPlatform.Tests.Services
{
    public class RuleServiceTests
    {
        private readonly Mock<IRuleRepository> _mockRepo;
        private readonly RuleService _service;

        public RuleServiceTests()
        {
            _mockRepo = new Mock<IRuleRepository>();
            _service = new RuleService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllRules_WhenRulesExist()
        {
            // Arrange
            var rule1 = Mock.Of<PricingRule>();
            var rule2 = Mock.Of<PricingRule>();

            var rules = new List<PricingRule> { rule1, rule2 };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(rules);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnRule_WhenRuleExists()
        {
            // Arrange
            var rule = Mock.Of<PricingRule>();
            _mockRepo.Setup(r => r.GetByIdAsync(rule.Id)).ReturnsAsync(rule);

            // Act
            var result = await _service.GetByIdAsync(rule.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenRuleDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((PricingRule?)null);

            // Act
            var result = await _service.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ShouldAssignNewId_WhenIdIsEmpty()
        {
            // Arrange
            var rule = Mock.Of<PricingRule>();
            _mockRepo.Setup(r => r.AddAsync(It.IsAny<PricingRule>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateAsync(rule);

            // Assert
            Assert.NotEqual(Guid.Empty, result.Id);
            _mockRepo.Verify(r => r.AddAsync(It.IsAny<PricingRule>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowKeyNotFoundException_WhenRuleDoesNotExist()
        {
            // Arrange
            var rule = Mock.Of<PricingRule>();
            _mockRepo.Setup(r => r.GetByIdAsync(rule.Id)).ReturnsAsync((PricingRule?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(rule));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRule_WhenRuleExists()
        {
            // Arrange
            var rule = Mock.Of<PricingRule>();
            _mockRepo.Setup(r => r.GetByIdAsync(rule.Id)).ReturnsAsync(rule);
            _mockRepo.Setup(r => r.UpdateAsync(rule)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.UpdateAsync(rule);

            // Assert
            Assert.Equal(rule, result);
            _mockRepo.Verify(r => r.UpdateAsync(rule), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowKeyNotFoundException_WhenRuleDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((PricingRule?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteRule_WhenRuleExists()
        {
            // Arrange
            var rule = Mock.Of<PricingRule>();
            _mockRepo.Setup(r => r.GetByIdAsync(rule.Id)).ReturnsAsync(rule);
            _mockRepo.Setup(r => r.DeleteAsync(rule.Id)).Returns(Task.CompletedTask);

            // Act
            await _service.DeleteAsync(rule.Id);

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(rule.Id), Times.Once);
        }
    }
}