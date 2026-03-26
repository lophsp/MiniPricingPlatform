using Microsoft.AspNetCore.Mvc;
using MiniPricingPlatform.Application.Interfaces;
using MiniPricingPlatform.API.DTOs;
using MiniPricingPlatform.API.Mapper;

namespace MiniPricingPlatform.API.Controllers;

[ApiController]
[Route("rules")]
public class RuleController : ControllerBase
{
    private readonly IRuleService _ruleService;

    public RuleController(IRuleService ruleService)
    {
        _ruleService = ruleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var rules = await _ruleService.GetAllAsync();
        var result = rules.Select(r => RuleMapper.ToDto(r));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var rule = await _ruleService.GetByIdAsync(id);
        if (rule == null) return NotFound();

        return Ok(RuleMapper.ToDto(rule));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RuleRequestDto dto)
    {
        var rule = RuleMapper.FromDto(dto);
        var created = await _ruleService.CreateAsync(rule);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, RuleMapper.ToDto(created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] RuleRequestDto dto)
    {
        var existing = await _ruleService.GetByIdAsync(id);
        if (existing == null) return NotFound();

        var rule = RuleMapper.FromDto(dto);
        rule.Id = id;

        var updated = await _ruleService.UpdateAsync(rule);
        return Ok(RuleMapper.ToDto(updated));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _ruleService.DeleteAsync(id);
        return NoContent();
    }

}