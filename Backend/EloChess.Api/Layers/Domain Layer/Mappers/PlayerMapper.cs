using EloChess.Api.DTOs;
using EloChess.Api.Models;
namespace EloChess.Api.Mappers;
public static class PlayerMapper
{
    public static PlayerDto ToDto(Player p)
    {
        return new PlayerDto
        {
            Id = p.Id,
            Username = p.Username,
            Email = p.Email,
            Password = p.Password,
            EloRating = p.EloRating,
            GamesPlayed = p.GamesPlayed,
            GamesWon = p.GamesWon,
            GamesLost = p.GamesLost,
            GamesDrawn = p.GamesDrawn
        };
    }

public static Player ToEntity(PlayerDto dto)
{
    return new Player
    {
        Id = dto.Id,
        Username = dto.Username,
        Email = dto.Email,
        Password = dto.Password,
        EloRating = dto.EloRating,
        GamesPlayed = dto.GamesPlayed,
        GamesWon = dto.GamesWon,
        GamesLost = dto.GamesLost,
        GamesDrawn = dto.GamesDrawn
    };
}
}
