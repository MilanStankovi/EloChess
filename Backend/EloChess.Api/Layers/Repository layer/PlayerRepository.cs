using EloChess.Api.DTOs;
using EloChess.Api.Mappers;
using EloChess.Api.Models;
using Microsoft.EntityFrameworkCore;
namespace EloChess.Api.Repository;
using EloChess.Api.Data;
public interface IPlayerRepository
{
    Task<PlayerDto> CreateAsync(PlayerDto dto);
    Task<PlayerDto?> GetByIdAsync(int id);
    Task<IEnumerable<PlayerDto>> GetAllAsync();
    Task UpdateAsync(PlayerDto dto);
    Task DeleteAsync(int id);
}

public class PlayerRepository : IPlayerRepository
{
    private readonly EloChessDbContext _context;

    public PlayerRepository(EloChessDbContext context)
    {
        _context = context;
    }

    public async Task<PlayerDto> CreateAsync(PlayerDto dto)
    {
        var entity = PlayerMapper.ToEntity(dto);

        _context.Players.Add(entity);            // Unit of Work
        await _context.SaveChangesAsync();

        return PlayerMapper.ToDto(entity);
    }

    public async Task<PlayerDto?> GetByIdAsync(int id)
    {
        var entity = await _context.Players.FindAsync(id);  // Identity Map
        return entity == null ? null : PlayerMapper.ToDto(entity);
    }

    public async Task<IEnumerable<PlayerDto>> GetAllAsync()
    {
        var entities = await _context.Players.ToListAsync();
        return entities.Select(PlayerMapper.ToDto);
    }

    public async Task UpdateAsync(PlayerDto dto)
    {
        var entity = await _context.Players.FindAsync(dto.Id);
        if (entity == null) return;

        entity.Username = dto.Username;
        entity.EloRating = dto.EloRating;
        entity.GamesPlayed = dto.GamesPlayed;
        entity.GamesWon = dto.GamesWon;
        entity.GamesLost = dto.GamesLost;
        entity.GamesDrawn = dto.GamesDrawn;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Players.FindAsync(id);
        if (entity == null) return;

        _context.Players.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
