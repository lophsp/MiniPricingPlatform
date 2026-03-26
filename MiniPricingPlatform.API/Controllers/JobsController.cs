using Microsoft.AspNetCore.Mvc;
using MiniPricingPlatform.API.DTOs;

namespace MiniPricingPlatform.API.Controllers;

[ApiController]
[Route("jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet("{jobId}")]
    public IActionResult Get(string jobId)
    {
        var job = _jobService.Get(jobId);

        if (job == null)
            return NotFound();

        var response = new JobDetailResponseDto
        {
            Status = job.Status,
            Results = job.Results.Select(r => new JobResultItemDto
            {
                Weight = r.Weight,
                Area = r.Area,
                Price = r.Price
            }).ToList()
        };

        return Ok(response);
    }
}