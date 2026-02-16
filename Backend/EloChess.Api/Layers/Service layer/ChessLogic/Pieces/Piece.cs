using ChessLogic;
using ChessLogic.Moves;

namespace ChessLogic.Pieces
{
    public enum Color
    {
        White,
        Black
    }

    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public abstract class Piece
    {
        public Color Color {get; }

        public PieceType Type {get; set;}

        public Position Position {get; set;}

        public IMoveStrategy MoveStrategy { get; private set; }

        public bool HasMoved { get; set; } = false;

        protected Piece(Color color, PieceType type, Position position, IMoveStrategy moveStrategy)
        {
            Color = color;
            Type = type;
            Position = position;
            MoveStrategy = moveStrategy;
        }

        public IEnumerable<Position> GetAvailableMoves(Board board)
        {
            return MoveStrategy.GetAvailableMoves(this, board);
        }
        
    }
}