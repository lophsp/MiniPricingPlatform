namespace MiniPricingPlatform.Application.Models;

public class JobResultItem
{
    public double Weight { get; set; }
    public string Area { get; set; } = null!;
    public decimal Price { get; set; }
}

public class JobDetailResponse
{
    public string Status { get; set; } = null!;
    public List<JobResultItem> Results { get; set; } = new();
}