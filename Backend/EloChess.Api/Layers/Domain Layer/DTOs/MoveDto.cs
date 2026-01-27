namespace EloChess.Api.DTOs;
public class MoveDto
{
    public int MoveNumber { get; set; }
    public string Notation { get; set; } = null!;
    public int PlayerId { get; set; }
}
