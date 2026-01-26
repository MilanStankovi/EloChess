namespace EloChess.Api.Models
{
public class Match
{
    public int Id { get; set; }

    public DateTime Started { get; set; }
    public DateTime? Ended { get; set; }

    public MatchStatus Status { get; set; }
    public MatchResult Result { get; set; }

    public ICollection<PlayerMatch> Players { get; set; } = new List<PlayerMatch>();
    public ICollection<Move> Moves { get; set; } = new List<Move>();
}

}