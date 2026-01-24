using Microsoft.EntityFrameworkCore;
using EloChess.Api.Models;

namespace EloChess.Api.Data
{
    public class EloChessContext : DbContext
    {
        public EloChessContext(DbContextOptions<EloChessContext> options) : base(options) { }

        public DbSet<Player> Players { get; set; }
    }
}
