using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Domain.Entities;

namespace MiniPricingPlatform.Application.Services;

public class RuleService : IRuleService
{
    private readonly IRuleRepository _repository;

    public RuleService(IRuleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PricingRule>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<PricingRule?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<PricingRule> CreateAsync(PricingRule rule)
    {
        if (rule.Id == Guid.Empty)
            rule.Id = Guid.NewGuid();

        await _repository.AddAsync(rule);
        return rule;
    }

    public async Task<PricingRule> UpdateAsync(PricingRule rule)
    {
        var existing = await _repository.GetByIdAsync(rule.Id);
        if (existing == null)
            throw new KeyNotFoundException($"Rule with Id {rule.Id} not found.");

        await _repository.UpdateAsync(rule);
        return rule;
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Rule with Id {id} not found.");

        await _repository.DeleteAsync(id);
    }
}