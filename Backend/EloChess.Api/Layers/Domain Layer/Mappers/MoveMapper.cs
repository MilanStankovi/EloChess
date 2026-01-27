using EloChess.Api.DTOs;
using EloChess.Api.Models;
namespace EloChess.Api.Mappers;
public static class MoveMapper
{
    public static MoveDto ToDto(Move m)
    {
        return new MoveDto
        {
            MoveNumber = m.MoveNumber,
            Notation = m.Notation,
            PlayerId = m.PlayerId
        };
    }

    public static Move ToEntity(MoveDto dto, int matchId)
    {
        return new Move
        {
            MatchId = matchId,
            PlayerId = dto.PlayerId,
            MoveNumber = dto.MoveNumber,
            Notation = dto.Notation,
            PlayedAt = DateTime.UtcNow
        };
    }
}
