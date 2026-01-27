using EloChess.Api.DTOs;
using EloChess.Api.Mappers;
using EloChess.Api.Models;
using Microsoft.EntityFrameworkCore;
using EloChess.Api.Data;
namespace EloChess.Api.Repository;
public interface IMatchRepository
{
    Task<MatchDto> CreateAsync(MatchDto dto);
    Task<MatchDto?> GetByIdAsync(int id);
    Task<IEnumerable<MatchDto>> GetAllAsync();
    Task UpdateAsync(MatchDto dto);
    Task DeleteAsync(int id);
}

public class MatchRepository : IMatchRepository
{
    private readonly EloChessDbContext _context;

    public MatchRepository(EloChessDbContext context)
    {
        _context = context;
    }

    public async Task<MatchDto> CreateAsync(MatchDto dto)
    {
        var entity = MatchMapper.ToEntity(dto);
        _context.Matches.Add(entity);
        await _context.SaveChangesAsync();
        return MatchMapper.ToDto(entity);
    }

    public async Task<MatchDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Matches
            .Include(m => m.Players)   // Lazy loading navigacije
            .Include(m => m.Moves)
            .FirstOrDefaultAsync(m => m.Id == id);

        return entity == null ? null : MatchMapper.ToDto(entity);
    }

    public async Task<IEnumerable<MatchDto>> GetAllAsync()
    {
        var entities = await _context.Matches.ToListAsync();
        return entities.Select(MatchMapper.ToDto);
    }

    public async Task UpdateAsync(MatchDto dto)
    {
        var entity = await _context.Matches.FindAsync(dto.Id);
        if (entity == null) return;

        entity.Started = dto.Started;
        entity.Ended = dto.Ended;
        entity.Status = dto.Status;
        entity.Result = dto.Result;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Matches.FindAsync(id);
        if (entity == null) return;

        _context.Matches.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
