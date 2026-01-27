using Microsoft.AspNetCore.Mvc;
using EloChess.Api.DTOs;
using EloChess.Api.Repository;
namespace EloChess.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PlayerMatchController : ControllerBase
{
    private readonly IPlayerMatchRepository _repo;

    public PlayerMatchController(IPlayerMatchRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("match/{matchId}")]
    public async Task<ActionResult<IEnumerable<PlayerMatchDto>>> GetPlayersByMatch(int matchId)
    {
        var players = await _repo.GetByMatchAsync(matchId);
        return Ok(players);
    }

    [HttpPost]
    public async Task<ActionResult<PlayerMatchDto>> AddPlayer([FromBody] PlayerMatchDto dto)
    {
        var pm = await _repo.CreateAsync(dto);
        return CreatedAtAction(nameof(GetPlayersByMatch), new { matchId = dto.MatchId }, pm);
    }

    [HttpDelete("match/{matchId}/player/{playerId}")]
    public async Task<IActionResult> RemovePlayer(int matchId, int playerId)
    {
        await _repo.DeleteAsync(playerId, matchId);
        return NoContent();
    }
}
