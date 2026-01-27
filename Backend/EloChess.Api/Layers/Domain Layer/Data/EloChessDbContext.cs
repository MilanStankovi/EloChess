using Microsoft.EntityFrameworkCore;
using EloChess.Api.Models;

namespace EloChess.Api.Data
{
    public class EloChessDbContext : DbContext
{
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<Match> Matches { get; set; } = null!;
    public DbSet<PlayerMatch> PlayerMatches { get; set; } = null!;
    public DbSet<Move> Moves { get; set; } = null!;

    public EloChessDbContext(DbContextOptions<EloChessDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite PK za PlayerMatch
        modelBuilder.Entity<PlayerMatch>()
            .HasKey(pm => new { pm.PlayerId, pm.MatchId });

        // Relacije
        modelBuilder.Entity<PlayerMatch>()
            .HasOne(pm => pm.Player)
            .WithMany(p => p.Matches)
            .HasForeignKey(pm => pm.PlayerId);

        modelBuilder.Entity<PlayerMatch>()
            .HasOne(pm => pm.Match)
            .WithMany(m => m.Players)
            .HasForeignKey(pm => pm.MatchId);

        modelBuilder.Entity<Move>()
            .HasOne(mv => mv.Player)
            .WithMany()
            .HasForeignKey(mv => mv.PlayerId);

        modelBuilder.Entity<Move>()
            .HasOne(mv => mv.Match)
            .WithMany(m => m.Moves)
            .HasForeignKey(mv => mv.MatchId);
    }
}

}
