using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Pieces;

namespace ChessLogic.Moves
{
    public interface IMoveStrategy
    {
        IEnumerable<Position> GetAvailableMoves(Piece piece, Board board);
    }
}