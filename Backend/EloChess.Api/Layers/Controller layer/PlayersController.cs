using Microsoft.AspNetCore.Mvc;
using EloChess.Api.DTOs;
using EloChess.Api.Repository;
[ApiController]
[Route("api/[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IPlayerRepository _repo;

    public PlayerController(IPlayerRepository repo)
    {
        _repo = repo;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetPlayer(int id)
    {
        var player = await _repo.GetByIdAsync(id);
        if (player == null) return NotFound();
        return Ok(player);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetAll()
    {
        var players = await _repo.GetAllAsync();
        return Ok(players);
    }

    [HttpPost]
    public async Task<ActionResult<PlayerDto>> CreatePlayer([FromBody] PlayerDto dto)
    {
        var player = await _repo.CreateAsync(dto);
        return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(int id, [FromBody] PlayerDto dto)
    {
        if (id != dto.Id) return BadRequest();
        await _repo.UpdateAsync(dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(int id)
    {
        await _repo.DeleteAsync(id);
        return NoContent();
    }
}
