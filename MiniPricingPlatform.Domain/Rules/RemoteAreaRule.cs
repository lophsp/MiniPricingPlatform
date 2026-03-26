using MiniPricingPlatform.Domain.Entities;
using MiniPricingPlatform.Domain.Models;

namespace MiniPricingPlatform.Domain.Rules;

public class RemoteAreaRule : PricingRule
{
    public string Area { get; set; } = "RemoteArea";
    public decimal Surcharge { get; set; }

    public override decimal Apply(decimal price, PricingInput input)
    {
        if (input.Area.Equals(Area, StringComparison.OrdinalIgnoreCase))
        {
            return price + Surcharge;
        }

        return price;
    }
}