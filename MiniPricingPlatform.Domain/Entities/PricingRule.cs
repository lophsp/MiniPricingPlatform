
using MiniPricingPlatform.Domain.Models;

namespace MiniPricingPlatform.Domain.Entities;

public abstract class PricingRule
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Priority { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime EffectiveTo { get; set; }
    public bool IsActive { get; set; }

    public abstract decimal Apply(decimal price, PricingInput input);
}