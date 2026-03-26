namespace MiniPricingPlatform.API.DTOs;

public class RuleRequestDto
{
    public string Type { get; set; } = null!;
    public int Priority { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime EffectiveTo { get; set; }
    public bool IsActive { get; set; }

    public decimal? Price { get; set; }              //  WeightTierRule
    public int? Min { get; set; }             //  WeightTierRule
    public int? Max { get; set; }             //  WeightTierRule
    public decimal? RemoteSurcharge { get; set; }   //  RemoteAreaRule
    public decimal? DiscountPercentage { get; set; } //  TimeWindowPromotionRule
    public TimeSpan? StartTime { get; set; }        //  TimeWindowPromotionRule
    public TimeSpan? EndTime { get; set; }          //  TimeWindowPromotionRule
}