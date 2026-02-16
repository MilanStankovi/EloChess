using System;
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

        // --- Stare osnovne funkcije koje si imao ---
        public static bool IsInside(int rank, int file) => rank >= 0 && rank < Size && file >= 0 && file < Size;
        public static bool IsInside(Position pos) => IsInside(pos.Rank, pos.File);
        public bool IsEmpty(Position pos) => Pieces.All(p => p.Position != pos);

        public bool IsEnemy(Position pos, Color color)
        {
            var piece = GetPiece(pos);
            return piece != null && piece.Color != color;
        }

        public Piece? GetPiece(Position pos)
        {
            if (!IsInside(pos)) return null;
            return Pieces.FirstOrDefault(p => p.Position == pos);
        }

        public void AddPiece(Piece piece)
        {
            if (!Pieces.Contains(piece)) Pieces.Add(piece);
        }

        public void RemovePiece(Piece piece)
        {
            if (Pieces.Contains(piece)) Pieces.Remove(piece);
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == Color.White ? Color.Black : Color.White;
        }

        // Vraćene funkcije koje su falile
        public IEnumerable<Piece> GetPieces(Color color) => Pieces.Where(p => p.Color == color);
        public IEnumerable<Piece> GetEnemyPieces(Color color) => Pieces.Where(p => p.Color != color);

        public Piece? GetKing(Color color)
        {
            return Pieces.FirstOrDefault(p => p.Type == PieceType.King && p.Color == color);
        }

        // --- Nova logika za validaciju poteza ---
        public bool IsLegalMove(Piece piece, Position to)
        {
            if (!IsInside(to)) return false;

            // 1. Osnovna geometrija i blokada (ovo već imaš)
            var moves = piece.GetAvailableMoves(this);
            if (!moves.Any(m => m.Rank == to.Rank && m.File == to.File)) return false;

            if (piece.Type != PieceType.Knight && !IsPathClear(piece.Position, to)) return false;

            var targetPiece = GetPiece(to);
            if (targetPiece != null && targetPiece.Color == piece.Color) return false;

            // 2. SIMULACIJA: "Šta ako odigram ovaj potez?"
            Position originalPos = piece.Position;
            Piece? capturedPiece = GetPiece(to);

            // Privremeno izvrši potez
            if (capturedPiece != null) Pieces.Remove(capturedPiece);
            piece.Position = to;

            // Proveri da li je tvoj kralj sada pod napadom
            bool isKingSafe = !IsKingInCheck(piece.Color);

            // VRATI SVE NAZAD (Ovo je ključno!)
            piece.Position = originalPos;
            if (capturedPiece != null) Pieces.Add(capturedPiece);

            return isKingSafe; // Ako kralj NIJE bezbedan, vrati false
        }

        // --- Nova metoda za IsPathClear ---
        public bool IsPathClear(Position from, Position to)
        {
            int dRank = to.Rank - from.Rank;
            int dFile = to.File - from.File;

            int stepRank = Math.Sign(dRank);
            int stepFile = Math.Sign(dFile);

            int currRank = from.Rank + stepRank;
            int currFile = from.File + stepFile;

            while (currRank != to.Rank || currFile != to.File)
            {
                if (GetPiece(new Position(currRank, currFile)) != null)
                    return false;

                currRank += stepRank;
                currFile += stepFile;
            }
            return true;
        }

        // --- Bezbedna provera šaha (bez rekurzije) ---
        public bool IsKingInCheck(Color color)
        {
            var king = GetKing(color);
            if (king == null) return false;

            foreach (var p in Pieces.Where(p => p.Color != color).ToList())
            {
                int dRank = Math.Abs(king.Position.Rank - p.Position.Rank);
                int dFile = Math.Abs(king.Position.File - p.Position.File);

                bool canAttack = p.Type switch
                {
                    PieceType.Bishop => dRank == dFile,
                    PieceType.Rook => dRank == 0 || dFile == 0,
                    PieceType.Knight => (dRank == 2 && dFile == 1) || (dRank == 1 && dFile == 2),
                    PieceType.Queen => dRank == dFile || dRank == 0 || dFile == 0,
                    PieceType.Pawn => dRank == 1 && dFile == 1, 
                    PieceType.King => dRank <= 1 && dFile <= 1,
                    _ => false
                };

                if (canAttack)
                {
                    if (p.Type == PieceType.Knight || IsPathClear(p.Position, king.Position))
                        return true;
                }
            }
            return false;
        }

        public string GetPositionKey()
        {
            var parts = new List<string>();
            foreach (var p in Pieces.OrderBy(p => p.Position.Rank).ThenBy(p => p.Position.File))
            {
                parts.Add($"{p.Type}-{p.Color}-{p.Position.Rank}-{p.Position.File}");
            }
            return string.Join("|", parts);
        }
    }
}