using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Pieces;

namespace ChessLogic.Moves
{
    public class KnightMoveStrategy : IMoveStrategy
    {
        private static readonly (int, int)[] Moves =
        {
            (2,1),(2,-1),(-2,1),(-2,-1),
            (1,2),(1,-2),(-1,2),(-1,-2)
        };

        public IEnumerable<Position> GetAvailableMoves(Piece piece, Board board)
        {
            var result = new List<Position>();

            foreach (var (dr, df) in Moves)
            {
                int r = piece.Position.Rank + dr;
                int f = piece.Position.File + df;

                if (!Board.IsInside(r, f)) continue;

                var pos = new Position(r, f);
                if (board.IsEmpty(pos) || board.IsEnemy(pos, piece.Color))
                    result.Add(pos);
            }

            return result;
        }
    }
}