using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public class Pawn : Piece
    {
        public bool EnPassantEligible { get; set; } = false;
        public Pawn(Color color, Position position)
            : base(color, PieceType.Pawn, position, new PawnMoveStrategy())
        {
            
        }
    }
}