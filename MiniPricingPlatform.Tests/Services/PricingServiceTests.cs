using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Application.Services;
using MiniPricingPlatform.Domain.Entities;
using MiniPricingPlatform.Domain.Models;
using MiniPricingPlatform.Domain.Rules;
using Moq;

namespace MiniPricingPlatform.Tests.Services;

public class PricingServiceTests
{
    [Fact]
    public async Task CalculateAsync_ShouldReturnCorrectPrice_ForWeightTierRule()
    {
        // Arrange
        var mockRepo = new Mock<IRuleRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<PricingRule>
        {
            new WeightTierRule { Min = 0, Max = 5, Price = 100, IsActive = true, Priority = 1, EffectiveFrom = DateTime.UtcNow.AddDays(-1), EffectiveTo = DateTime.UtcNow.AddDays(1) }
        });

        var service = new PricingService(mockRepo.Object);
        var input = new PricingInput { Weight = 3, Area = "City", RequestTime = DateTime.UtcNow };

        // Act
        var result = await service.CalculateAsync(input);

        // Assert
        Assert.Equal(100, result);
    }

    [Fact]
    public async Task CalculateAsync_ShouldApplyRemoteSurcharge_WhenAreaMatches()
    {
        // Arrange
        var mockRepo = new Mock<IRuleRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<PricingRule>
        {
            new RemoteAreaRule { Area = "RemoteArea", Surcharge = 50, IsActive = true, Priority = 1, EffectiveFrom = DateTime.UtcNow.AddDays(-1), EffectiveTo = DateTime.UtcNow.AddDays(1) }
        });

        var service = new PricingService(mockRepo.Object);
        var input = new PricingInput { Weight = 3, Area = "RemoteArea", RequestTime = DateTime.UtcNow };

        // Act
        var result = await service.CalculateAsync(input);

        // Assert
        Assert.Equal(50, result);
    }
}