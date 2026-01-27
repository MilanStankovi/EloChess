namespace EloChess.Api.DTOs;

public class PlayerMatchDto
{
    public int PlayerId { get; set; }
    public int MatchId { get; set; }
    public bool IsWhite { get; set; }
    public int EloBefore { get; set; }
    public int EloAfter { get; set; }
}
