using EloChess.Api.DTOs;
using EloChess.Api.Models;
namespace EloChess.Api.Mappers;
public static class MatchMapper
{
    public static MatchDto ToDto(Match m)
    {
        return new MatchDto
        {
            Id = m.Id,
            Started = m.Started,
            Ended = m.Ended,
            Status = m.Status,
            Result = m.Result
        };
    }

    public static Match ToEntity(MatchDto dto)
    {
        return new Match
        {
            Id = dto.Id,
            Started = dto.Started,
            Ended = dto.Ended,
            Status = dto.Status,
            Result = dto.Result
        };
    }
}
