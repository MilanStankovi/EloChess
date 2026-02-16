using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Pieces;

namespace ChessLogic.Moves
{
    public class QueenMoveStrategy : IMoveStrategy
    {
        public IEnumerable<Position> GetAvailableMoves(Piece piece, Board board)
        {
            var moves = new List<Position>();

            int[][] directions =
            {
                new[] { 1, 0 }, new[] { -1, 0 }, new[] { 0, 1 }, new[] { 0, -1 },
                new[] { 1, 1 }, new[] { 1, -1 }, new[] { -1, 1 }, new[] { -1, -1 }
            };

            foreach (var d in directions)
            {
                int r = piece.Position.Rank + d[0];
                int f = piece.Position.File + d[1];

                while (Board.IsInside(r, f))
                {
                    var pos = new Position(r, f);

                    if (board.IsEmpty(pos))
                        moves.Add(pos);
                    else
                    {
                        if (board.IsEnemy(pos, piece.Color))
                            moves.Add(pos);
                        break;
                    }

                    r += d[0];
                    f += d[1];
                }
            }

            return moves;
        }
    }
}