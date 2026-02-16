using ChessLogic.Commands;

namespace ChessLogic.Game
{
    public enum GameResult
    {
        None,
        Ongoing,
        WhiteWins,
        BlackWins,
        DrawStalemate,
        DrawInsufficientMaterial,
        DrawThreefold,
        DrawFiftyMove
    }

    public interface IGameEndObserver
    {
        void OnMovePlayed(MoveCommand command);
        void OnGameEnded(GameResult result);
    }

}

