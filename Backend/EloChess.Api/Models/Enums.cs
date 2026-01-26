namespace EloChess.Api.Models
{
    public enum MatchResult
    {
        Ongoing = 0,
        WhiteWin = 1,
        BlackWin = 2,
        Draw = 3
    }

    public enum MatchStatus
    {
        WaitingForPlayers = 0,
        InProgress = 1,
        Finished = 2,
        Abandoned = 3
    }
}
