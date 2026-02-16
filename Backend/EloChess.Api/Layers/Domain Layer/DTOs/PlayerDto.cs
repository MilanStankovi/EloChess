namespace EloChess.Api.DTOs
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
        public int EloRating { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesLost { get; set; }
        public int GamesDrawn { get; set; }

        public string Password { get; set; } = null!;
    }
}