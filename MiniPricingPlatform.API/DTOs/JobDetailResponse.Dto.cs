namespace MiniPricingPlatform.API.DTOs;
    public class JobResultItemDto
    {
        public double Weight { get; set; }
        public string Area { get; set; } = null!;
        public decimal Price { get; set; }
    }

    public class JobDetailResponseDto
    {
        public string Status { get; set; } = null!;
        public List<JobResultItemDto> Results { get; set; } = new();
    }