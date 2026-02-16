using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Pieces;

namespace ChessLogic.Moves
{
    public class KingMoveStrategy : IMoveStrategy
    {
        public IEnumerable<Position> GetAvailableMoves(Piece piece, Board board)
        {
            var moves = new List<Position>();

            for (int dr = -1; dr <= 1; dr++)
            for (int df = -1; df <= 1; df++)
            {
                if (dr == 0 && df == 0) continue;

                int r = piece.Position.Rank + dr;
                int f = piece.Position.File + df;

                if (!Board.IsInside(r, f)) continue;

                var pos = new Position(r, f);
                if (board.IsEmpty(pos) || board.IsEnemy(pos, piece.Color))
                    moves.Add(pos);
            }

            return moves;
        }
    }
}