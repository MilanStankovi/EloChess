using Microsoft.AspNetCore.Mvc;
using EloChess.Api.DTOs;
using EloChess.Api.Repository;
namespace EloChess.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MatchController : ControllerBase
{
    private readonly IMatchRepository _repo;

    public MatchController(IMatchRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MatchDto>> GetMatch(int id)
    {
        var match = await _repo.GetByIdAsync(id);
        if (match == null) return NotFound();
        return Ok(match);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MatchDto>>> GetAll()
    {
        var matches = await _repo.GetAllAsync();
        return Ok(matches);
    }

    [HttpPost]
    public async Task<ActionResult<MatchDto>> CreateMatch([FromBody] MatchDto dto)
    {
        var match = await _repo.CreateAsync(dto);
        return CreatedAtAction(nameof(GetMatch), new { id = match.Id }, match);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMatch(int id, [FromBody] MatchDto dto)
    {
        if (id != dto.Id) return BadRequest();
        await _repo.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMatch(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
