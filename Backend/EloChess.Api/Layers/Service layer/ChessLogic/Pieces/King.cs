using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public class King : Piece
    {
        public King(Color color, Position position)
            : base(color, PieceType.Knight, position, new KingMoveStrategy())
        {
            
        }
    }
}