using MiniPricingPlatform.Domain.Entities;
using MiniPricingPlatform.Domain.Models;

namespace MiniPricingPlatform.Domain.Rules;

public class TimeWindowPromotionRule : PricingRule
{
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTIme { get; set; }
    public decimal DiscountPercent { get; set; }

    public override decimal Apply(decimal price, PricingInput input)
    {
        if (price <= 0)
            return price;

        var reqTime = input.RequestTime.ToUniversalTime().TimeOfDay;

        if (reqTime >= StartTime && reqTime <= EndTIme)
        {
            var discount = price * (DiscountPercent / 100);
            return price - discount;
        }

        return price;
    }
}