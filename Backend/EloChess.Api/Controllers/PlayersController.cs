using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EloChess.Api.Data;
using EloChess.Api.Models;

namespace EloChess.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly EloChessDbContext _context;

        public PlayersController(EloChessDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPlayers), new { id = player.Id }, player);
        }
    }
}
