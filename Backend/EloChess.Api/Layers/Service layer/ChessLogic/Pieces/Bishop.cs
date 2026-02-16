using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public class Bishop : Piece
    {
        public Bishop(Color color, Position position)
            : base(color, PieceType.Bishop, position, new BishopMoveStrategy())
        {
            
        }
    }
}