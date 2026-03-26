
using MiniPricingPlatform.Domain.Models;
using MiniPricingPlatform.Domain.Rules;

namespace MiniPricingPlatform.API.Tests.Rules;

public class RulesTest
{
    //  WeightTierRule
    [Fact]
    public void WeightTierRule_ShouldReturnPrice_WhenWeightInRange()
    {
        var rule = new WeightTierRule { Min = 0, Max = 5, Price = 100 };
        var input = new PricingInput { Weight = 3 };

        var result = rule.Apply(0, input);

        Assert.Equal(100, result);
    }

    // RemoteAreaSurcharge
    [Fact]
    public void RemoteAreaRule_ShouldAddSurcharge_WhenAreaMatches()
    {
        var rule = new RemoteAreaRule { Area = "RemoteArea", Surcharge = 50 };
        var input = new PricingInput { Area = "RemoteArea" };

        var result = rule.Apply(100, input);

        Assert.Equal(150, result);
    }

    // TimeWindowPromotion
    [Fact]
    public void TimeWindowPromotionRule_ShouldApplyDiscount_WhenWithinTime()
    {
        var rule = new TimeWindowPromotionRule
        {
            StartTime = TimeSpan.Parse("18:00:00"),
            EndTIme = TimeSpan.Parse("22:00:00"),
            DiscountPercent = 20
        };
        var input = new PricingInput { RequestTime = DateTime.Parse("2026-03-25T19:00:00Z") };

        var result = rule.Apply(100, input);

        Assert.Equal(80, result);
    }
}