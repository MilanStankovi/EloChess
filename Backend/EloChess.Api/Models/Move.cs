namespace EloChess.Api.Models{
    public class Move
    {
        public int Id { get; set; }

        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

        public int PlayerId { get; set; }
        public Player Player { get; set; } = null!;

        public int MoveNumber { get; set; }
        public string Notation { get; set; } = null!;  // "e4", "Nf3", "O-O"

        public DateTime PlayedAt { get; set; }
    }
}