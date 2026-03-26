using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Domain.Models;

namespace MiniPricingPlatform.Application.Services;

public class PricingService : IPricingService
{
    private readonly IRuleRepository _repo;

    public PricingService(IRuleRepository repo)
    {
        _repo = repo;
    }

    public async Task<decimal> CalculateAsync(PricingInput input)
    {
        var rules = await _repo.GetAllAsync();

        var requestUtc = input.RequestTime.ToUniversalTime();
        var active = rules
            .Where(r => r.IsActive &&
                        requestUtc >= r.EffectiveFrom &&
                        requestUtc <= r.EffectiveTo)
            .OrderBy(r => r.Priority);

        decimal price = 0;

        foreach (var rule in active)
        {
            price = rule.Apply(price, input);
        }

        return price;
    }
}