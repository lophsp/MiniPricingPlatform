using Microsoft.AspNetCore.Mvc;
using MiniPricingPlatform.API.DTOs;

namespace MiniPricingPlatform.API.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Check()
    {
        return Ok(new HealthResponseDto { Status = "ok" });
    }
}