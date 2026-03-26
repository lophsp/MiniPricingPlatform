namespace MiniPricingPlatform.API.DTOs;

public class RuleResponseDto
{
    public Guid Id { get; set; }
    public string Type { get; set; } = null!;
    public int Priority { get; set; }
    public DateTime EffectiveFrom { get; set; }
    public DateTime EffectiveTo { get; set; }
    public bool IsActive { get; set; }
}