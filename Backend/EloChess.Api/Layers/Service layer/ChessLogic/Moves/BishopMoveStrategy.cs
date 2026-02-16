using System.Collections.Generic;
using ChessLogic;
using ChessLogic.Pieces;

namespace ChessLogic.Moves
{
    public class BishopMoveStrategy : IMoveStrategy
    {
        public IEnumerable<Position> GetAvailableMoves(Piece piece, Board board)
        {
            var moves = new List<Position>();

            int[] dirs = { -1, 1 };

            foreach (int dr in dirs)
            foreach (int df in dirs)
            {
                int r = piece.Position.Rank + dr;
                int f = piece.Position.File + df;

                while (Board.IsInside(r, f))
                {
                    var pos = new Position(r, f);

                    if (board.IsEmpty(pos))
                    {
                        moves.Add(pos);
                    }
                    else
                    {
                        if (board.IsEnemy(pos, piece.Color))
                            moves.Add(pos);
                        break;
                    }

                    r += dr;
                    f += df;
                }
            }

            return moves;
        }
    }
}