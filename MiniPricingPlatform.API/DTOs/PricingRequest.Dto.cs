namespace MiniPricingPlatform.API.DTOs;

public class PricingRequestDto
{
    public double Weight { get; set; }
    public string Area { get; set; } = string.Empty;

    public DateTime RequestTime { get; set; }
}