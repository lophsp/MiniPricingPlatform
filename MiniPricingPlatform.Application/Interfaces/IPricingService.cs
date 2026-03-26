using MiniPricingPlatform.Domain.Models;

namespace MiniPricingPlatform.Application.Interfaces;

public interface IPricingService
{
    Task<decimal> CalculateAsync(PricingInput input);
}