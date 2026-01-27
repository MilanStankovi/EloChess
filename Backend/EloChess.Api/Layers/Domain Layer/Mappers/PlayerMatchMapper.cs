using EloChess.Api.DTOs;
using EloChess.Api.Models;
namespace EloChess.Api.Mappers;
public static class PlayerMatchMapper
{
    public static PlayerMatchDto ToDto(PlayerMatch pm)
    {
        return new PlayerMatchDto
        {
            PlayerId = pm.PlayerId,
            MatchId = pm.MatchId,
            IsWhite = pm.IsWhite,
            EloBefore = pm.EloBefore,
            EloAfter = pm.EloAfter
        };
    }

    public static PlayerMatch ToEntity(PlayerMatchDto dto)
    {
        return new PlayerMatch
        {
            PlayerId = dto.PlayerId,
            MatchId = dto.MatchId,
            IsWhite = dto.IsWhite,
            EloBefore = dto.EloBefore,
            EloAfter = dto.EloAfter
        };
    }
}
