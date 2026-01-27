namespace EloChess.Api.Models{
    public class PlayerMatch
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

        public bool IsWhite { get; set; }

        public int EloBefore { get; set; }
        public int EloAfter { get; set; }
    }
}