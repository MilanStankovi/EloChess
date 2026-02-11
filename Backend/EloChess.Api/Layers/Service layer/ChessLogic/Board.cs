using System.Collections.Generic;
using System.Linq;
using ChessLogic.Pieces;
using ChessLogic.Commands;

namespace ChessLogic
{
    public class Board
    {
        public List<Piece> Pieces { get; } = new List<Piece>();

        public Color CurrentPlayer { get; private set; } = Color.White;

        public const int Size = 8;

        
        public static bool IsInside(int rank, int file)
        {
            return rank >= 0 && rank < Size &&
                   file >= 0 && file < Size;
        }

        public static bool IsInside(Position pos)
        {
            return IsInside(pos.Rank, pos.File);
        }

       
        public bool IsEmpty(Position pos)
        {
            return Pieces.All(p => p.Position != pos);
        }

       
        public bool IsEnemy(Position pos, Color color)
        {
            var piece = GetPiece(pos);
            return piece != null && piece.Color != color;
        }

        
        public Piece? GetPiece(Position pos)
        {
            return Pieces.FirstOrDefault(p => p.Position == pos);
        }


        public void AddPiece(Piece piece)
        {
            if (!Pieces.Contains(piece))
                Pieces.Add(piece);
        }

        public void RemovePiece(Piece piece)
        {
            if (Pieces.Contains(piece))
                Pieces.Remove(piece);
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Color.White
                ? Color.Black
                : Color.White;
        }

        public IEnumerable<Piece> GetPieces(Color color)
        {
            return Pieces.Where(p => p.Color == color);
        }

        public IEnumerable<Piece> GetEnemyPieces(Color color)
        {
            return Pieces.Where(p => p.Color != color);
        }

        public Piece GetKing(Color color)
        {
            return Pieces.First(p => p.Type == PieceType.King && p.Color == color);
        }

        public bool IsLegalMove(Piece piece, Position target)
        {
            var cmd = new MoveCommand(this, piece, target);
            cmd.Execute();

            bool ok = !IsKingInCheck(piece.Color);

            cmd.Undo();

            return ok;
        }

        public string GetPositionKey()
        {
            var parts = new List<string>();

            foreach (var p in Pieces
                .OrderBy(p => p.Position.Rank)
                .ThenBy(p => p.Position.File))
            {
                parts.Add($"{p.Type}-{p.Color}-{p.Position.Rank}-{p.Position.File}");
            }

            return string.Join("|", parts);
        }

        public bool IsKingInCheck(Color color)
        {
            var king = Pieces.First(p =>
                p.Type == PieceType.King &&
                p.Color == color);

            foreach (var p in Pieces.Where(p => p.Color != color))
            {
                var moves = p.GetAvailableMoves(this);

                if (moves.Any(m => m == king.Position))
                    return true;
            }

            return false;
        }
    }
}

