using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.Application.Services;
using MiniPricingPlatform.Infrastructure.Repositories;

namespace MiniPricingPlatform.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IPricingService, PricingService>();
        services.AddScoped<IRuleRepository, RuleRepository>();
        services.AddScoped<IJobService, JobService>();
        services.AddScoped<IRuleService, RuleService>();

        return services;
    }
}