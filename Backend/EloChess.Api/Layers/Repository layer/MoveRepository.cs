using EloChess.Api.DTOs;
using EloChess.Api.Mappers;
using EloChess.Api.Models;
using Microsoft.EntityFrameworkCore;
using EloChess.Api.Data;
namespace EloChess.Api.Repository;
public interface IMoveRepository
{
    Task<MoveDto> CreateAsync(MoveDto dto, int matchId);
    Task<IEnumerable<MoveDto>> GetByMatchAsync(int matchId);
    Task DeleteAsync(int id);
}

public class MoveRepository : IMoveRepository
{
    private readonly EloChessDbContext _context;

    public MoveRepository(EloChessDbContext context)
    {
        _context = context;
    }

    public async Task<MoveDto> CreateAsync(MoveDto dto, int matchId)
    {
        var entity = MoveMapper.ToEntity(dto, matchId);
        _context.Moves.Add(entity);
        await _context.SaveChangesAsync();
        return MoveMapper.ToDto(entity);
    }

    public async Task<IEnumerable<MoveDto>> GetByMatchAsync(int matchId)
    {
        var moves = await _context.Moves
            .Where(m => m.MatchId == matchId)
            .OrderBy(m => m.MoveNumber)
            .ToListAsync();

        return moves.Select(MoveMapper.ToDto);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Moves.FindAsync(id);
        if (entity == null) return;

        _context.Moves.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
