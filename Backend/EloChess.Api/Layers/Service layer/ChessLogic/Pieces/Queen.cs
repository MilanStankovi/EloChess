using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color, Position position)
            : base(color, PieceType.Queen, position, new QueenMoveStrategy())
        {
            
        }
    }
}