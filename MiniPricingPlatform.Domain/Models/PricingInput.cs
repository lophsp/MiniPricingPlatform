namespace MiniPricingPlatform.Domain.Models;

public class PricingInput
{
    public double Weight { get; set; }
    public string Area { get; set; } = string.Empty;
    public DateTime RequestTime { get; set; }
}