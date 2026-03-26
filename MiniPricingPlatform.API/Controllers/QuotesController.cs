using Microsoft.AspNetCore.Mvc;
using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.API.DTOs;
using MiniPricingPlatform.Domain.Models;

namespace MiniPricingPlatform.API.Controllers;

[ApiController]
[Route("quotes")]
public class QuotesController : ControllerBase
{
    private readonly IPricingService _pricingService;
    private readonly IJobService _jobService;

    public QuotesController(IPricingService pricingService, IJobService jobService)
    {
        _pricingService = pricingService;
        _jobService = jobService;
    }

    [HttpPost("price")]
    public async Task<IActionResult> Calculate([FromBody] PricingRequestDto request)
    {
        var req = new PricingInput
        {
            Weight = request.Weight,
            Area = request.Area,
            RequestTime = request.RequestTime
        };

        var price = await _pricingService.CalculateAsync(req);

        return Ok(new PricingResponseDto { Price = price });
    }
    
    [HttpPost("bulk")]
    public IActionResult Bulk([FromBody] BulkRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Type))
            return BadRequest("Type is required");

        var jobId = _jobService.CreateJob(request.Type, request.Items);

        return Ok(new JobResponseDto { JobId = jobId });
    }
}