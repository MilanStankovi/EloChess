using EloChess.Api.Models;

namespace EloChess.Api.DTOs;
public class MatchDto
{
    public int Id { get; set; }
    public DateTime Started { get; set; }
    public DateTime? Ended { get; set; }
    public MatchStatus Status { get; set; }
    public MatchResult Result { get; set; }
}
