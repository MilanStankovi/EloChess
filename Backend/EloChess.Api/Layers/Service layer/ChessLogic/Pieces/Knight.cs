using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color, Position position)
            : base(color, PieceType.Knight, position, new KnightMoveStrategy())
        {
            
        }
    }
}