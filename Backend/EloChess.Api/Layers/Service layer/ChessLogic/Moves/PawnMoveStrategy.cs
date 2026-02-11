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

            // 1 polje napred
            var oneStep = new Position(r + direction, f);
            if (Board.IsInside(oneStep) && board.IsEmpty(oneStep))
            {
                moves.Add(oneStep);

                // 2 polja napred sa početne pozicije
                var twoStep = new Position(r + 2 * direction, f);
                if (r == startRank &&
                    Board.IsInside(twoStep) &&
                    board.IsEmpty(twoStep))
                {
                    moves.Add(twoStep);
                }
            }

            // dijagonalno uzimanje
            foreach (int df in new[] { -1, 1 })
            {
                int newRank = r + direction;
                int newFile = f + df;

                if (!Board.IsInside(newRank, newFile))
                    continue;

                var diag = new Position(newRank, newFile);
                if (board.IsEnemy(diag, piece.Color))
                {
                    moves.Add(diag);
                }
            }

            return moves;
        }
    }
}
