using EloChess.Api.DTOs;
using EloChess.Api.Mappers;
using EloChess.Api.Models;
using Microsoft.EntityFrameworkCore;
using EloChess.Api.Data;
namespace EloChess.Api.Repository;
public interface IPlayerMatchRepository
{
    Task<PlayerMatchDto> CreateAsync(PlayerMatchDto dto);
    Task<IEnumerable<PlayerMatchDto>> GetByMatchAsync(int matchId);
    Task DeleteAsync(int playerId, int matchId);
}

public class PlayerMatchRepository : IPlayerMatchRepository
{
    private readonly EloChessDbContext _context;

    public PlayerMatchRepository(EloChessDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerMatchDto> CreateAsync(PlayerMatchDto dto)
    {
        var entity = PlayerMatchMapper.ToEntity(dto);
        _context.PlayerMatches.Add(entity);
        await _context.SaveChangesAsync();
        return PlayerMatchMapper.ToDto(entity);
    }

    public async Task<IEnumerable<PlayerMatchDto>> GetByMatchAsync(int matchId)
    {
        var entities = await _context.PlayerMatches
            .Where(pm => pm.MatchId == matchId)
            .ToListAsync();

        return entities.Select(PlayerMatchMapper.ToDto);
    }

    public async Task DeleteAsync(int playerId, int matchId)
    {
        var entity = await _context.PlayerMatches
            .FindAsync(playerId, matchId);
        if (entity == null) return;

        _context.PlayerMatches.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
