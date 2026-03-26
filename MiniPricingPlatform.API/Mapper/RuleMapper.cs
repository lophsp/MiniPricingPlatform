using MiniPricingPlatform.API.DTOs;
using MiniPricingPlatform.Domain.Entities;
using MiniPricingPlatform.Domain.Rules;

namespace MiniPricingPlatform.API.Mapper;

public static class RuleMapper
{
    public static RuleResponseDto ToDto(PricingRule rule)
    {
        switch (rule)
        {
            case WeightTierRule w:
                return MapWeightRule(w);
            case RemoteAreaRule r:
                return MapRemoteRule(r);
            case TimeWindowPromotionRule t:
                return MapTimeWindowRule(t);
            default:
                return MapUnknownRule(rule);
        }
    }

    public static PricingRule FromDto(RuleRequestDto dto)
    {
        var type = dto.Type?.ToLower() ?? "";

        switch (type)
        {
            case "weighttier":
                return CreateWeightRule(dto);
            case "remoteareasurcharge":
                return CreateRemoteRule(dto);
            case "timewindowpromotion":
                return CreateTimeWindowRule(dto);
            default:
                throw new ArgumentException($"Unknown rule type: {dto.Type}");
        }
    }

    private static RuleResponseDto MapWeightRule(WeightTierRule rule)
    {
        var dto = new RuleResponseDto();
        dto.Id = rule.Id;
        dto.Type = "weighttier";
        dto.Priority = rule.Priority;
        dto.EffectiveFrom = rule.EffectiveFrom;
        dto.EffectiveTo = rule.EffectiveTo;
        dto.IsActive = rule.IsActive;
        return dto;
    }

    private static RuleResponseDto MapRemoteRule(RemoteAreaRule rule)
    {
        var dto = new RuleResponseDto();
        dto.Id = rule.Id;
        dto.Type = "remotearea";
        dto.Priority = rule.Priority;
        dto.EffectiveFrom = rule.EffectiveFrom;
        dto.EffectiveTo = rule.EffectiveTo;
        dto.IsActive = rule.IsActive;
        return dto;
    }

    private static RuleResponseDto MapTimeWindowRule(TimeWindowPromotionRule rule)
    {
        var dto = new RuleResponseDto();
        dto.Id = rule.Id;
        dto.Type = "timewindow";
        dto.Priority = rule.Priority;
        dto.EffectiveFrom = rule.EffectiveFrom;
        dto.EffectiveTo = rule.EffectiveTo;
        dto.IsActive = rule.IsActive;
        return dto;
    }

    private static RuleResponseDto MapUnknownRule(PricingRule rule)
    {
        var dto = new RuleResponseDto();
        dto.Id = rule.Id;
        dto.Type = "unknown";
        dto.Priority = rule.Priority;
        dto.EffectiveFrom = rule.EffectiveFrom;
        dto.EffectiveTo = rule.EffectiveTo;
        dto.IsActive = rule.IsActive;
        return dto;
    }
    private static WeightTierRule CreateWeightRule(RuleRequestDto dto)
    {
        var rule = new WeightTierRule();
        rule.Priority = dto.Priority;
        rule.EffectiveFrom = dto.EffectiveFrom;
        rule.EffectiveTo = dto.EffectiveTo;
        rule.IsActive = dto.IsActive;
        rule.Price = dto.Price ?? 0;
        rule.Min = dto.Min ?? 0;
        rule.Max = dto.Max ?? 0;
        return rule;
    }

    private static RemoteAreaRule CreateRemoteRule(RuleRequestDto dto)
    {
        var rule = new RemoteAreaRule();
        rule.Priority = dto.Priority;
        rule.EffectiveFrom = dto.EffectiveFrom;
        rule.EffectiveTo = dto.EffectiveTo;
        rule.IsActive = dto.IsActive;
        rule.Surcharge = dto.RemoteSurcharge ?? 0;
        return rule;
    }

    private static TimeWindowPromotionRule CreateTimeWindowRule(RuleRequestDto dto)
    {
        var rule = new TimeWindowPromotionRule();
        rule.Priority = dto.Priority;
        rule.EffectiveFrom = dto.EffectiveFrom;
        rule.EffectiveTo = dto.EffectiveTo;
        rule.IsActive = dto.IsActive;
        rule.DiscountPercent = dto.DiscountPercentage ?? 0;
        rule.StartTime = dto.StartTime ?? TimeSpan.Zero;
        rule.EndTIme = dto.EndTime ?? TimeSpan.Zero;
        return rule;
    }
}