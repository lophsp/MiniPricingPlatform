namespace MiniPricingPlatform.API.DTOs;

public class BulkRequestDto
{
    public string Type { get; set; } = "json";
    public object Items { get; set; } = new object();
}