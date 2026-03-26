namespace MiniPricingPlatform.Application.Models;

public class BulkItemInput
{
    public double Weight { get; set; }
    public string Area { get; set; } = string.Empty;
    public DateTime RequestTime { get; set; }
}