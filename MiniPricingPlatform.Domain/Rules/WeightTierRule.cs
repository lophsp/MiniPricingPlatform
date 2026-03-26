using MiniPricingPlatform.Domain.Entities;
using MiniPricingPlatform.Domain.Models;

namespace MiniPricingPlatform.Domain.Rules;

public class WeightTierRule : PricingRule
{
    public double Min { get; set; }
    public double Max { get; set; }
    public decimal Price { get; set; }

    public override decimal Apply(decimal price, PricingInput input)
    {
        if (input.Weight >= Min && input.Weight <= Max)
        {
            return Price;
        }

        return price;
    }

}