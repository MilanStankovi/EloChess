using Microsoft.AspNetCore.Mvc;
using EloChess.Api.DTOs;
using EloChess.Api.Repository;
namespace EloChess.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MoveController : ControllerBase
{
    private readonly IMoveRepository _repo;

    public MoveController(IMoveRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("match/{matchId}")]
    public async Task<ActionResult<IEnumerable<MoveDto>>> GetMovesByMatch(int matchId)
    {
        var moves = await _repo.GetByMatchAsync(matchId);
        return Ok(moves);
    }

    [HttpPost("match/{matchId}")]
    public async Task<ActionResult<MoveDto>> CreateMove(int matchId, [FromBody] MoveDto dto)
    {
        var move = await _repo.CreateAsync(dto, matchId);
        return CreatedAtAction(nameof(GetMovesByMatch), new { matchId = matchId }, move);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMove(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
