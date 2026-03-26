using MiniPricingPlatform.Domain.Entities;

namespace MiniPricingPlatform.Application.Interfaces;

public interface IRuleService
{
    Task<List<PricingRule>> GetAllAsync();
    Task<PricingRule?> GetByIdAsync(Guid id);
    Task<PricingRule> CreateAsync(PricingRule rule);
    Task<PricingRule> UpdateAsync(PricingRule rule);
    Task DeleteAsync(Guid id);
}