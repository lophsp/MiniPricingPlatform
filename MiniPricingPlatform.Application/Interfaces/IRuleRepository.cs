using MiniPricingPlatform.Domain.Entities;

namespace MiniPricingPlatform.Application.Interfaces;


public interface IRuleRepository
{
    Task<List<PricingRule>> GetAllAsync();
    Task<PricingRule?> GetByIdAsync(Guid id);
    Task AddAsync(PricingRule rule);
    Task UpdateAsync(PricingRule rule);
    Task DeleteAsync(Guid id);
    Task SaveAllAsync(List<PricingRule> rules);
}