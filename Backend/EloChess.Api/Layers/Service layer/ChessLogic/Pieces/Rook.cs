using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public class Rook : Piece
    {
        public Rook(Color color, Position position)
            : base(color, PieceType.Rook, position, new RookMoveStrategy())
        {
            
        }
    }
}