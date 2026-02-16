using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Pieces;

namespace ChessLogic.Moves
{
    public class PawnMoveStrategy : IMoveStrategy
    {
        public IEnumerable<Position> GetAvailableMoves(Piece piece, Board board)
        {
            var moves = new List<Position>();
            int direction = piece.Color == Color.White ? 1 : -1;
            int startRank = piece.Color == Color.White ? 1 : 6;
            int r = piece.Position.Rank;
            int f = piece.Position.File;

            // 1. Pomeranje unapred (jedno polje)
            var oneStep = new Position(r + direction, f);
            if (Board.IsInside(oneStep) && board.IsEmpty(oneStep))
            {
                moves.Add(oneStep);

                // 2. Pomeranje unapred (dva polja) - Proverava i polje između!
                var twoStep = new Position(r + 2 * direction, f);
                if (r == startRank && Board.IsInside(twoStep) && board.IsEmpty(twoStep))
                {
                    moves.Add(twoStep);
                }
            }

            // 3. Dijagonalno uzimanje i En Passant
            foreach (int df in new[] { -1, 1 })
            {
                int newRank = r + direction;
                int newFile = f + df;

                if (!Board.IsInside(newRank, newFile)) continue;

                var targetPos = new Position(newRank, newFile);
                
                // Standardno uzimanje
                if (board.IsEnemy(targetPos, piece.Color))
                {
                    moves.Add(targetPos);
                }
                // En Passant logika
                else if (board.IsEmpty(targetPos))
                {
                    var sidePiece = board.GetPiece(new Position(r, newFile));
                    if (sidePiece is Pawn enemyPawn && enemyPawn.Color != piece.Color && enemyPawn.EnPassantEligible)
                    {
                        moves.Add(targetPos);
                    }
                }
            }
            return moves;
        }
    }
}
