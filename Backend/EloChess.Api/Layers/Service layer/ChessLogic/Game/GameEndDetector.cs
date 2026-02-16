using System;
using System.Collections.Generic;
using System.Linq;
using ChessLogic.Commands;
using ChessLogic.Pieces;

namespace ChessLogic.Game
{
    public class GameEndDetector : IGameEndObserver
    {
        private readonly Board board;
        private readonly List<string> positionHistory = new();

        private int halfMoveClock = 0;

        public GameEndDetector(Board board)
        {
            this.board = board;
        }

        // ---------------- OBSERVER ----------------

        public void OnMovePlayed(MoveCommand command)
        {
            UpdateHistory();
            UpdateHalfMoveClock(command);

            var result = CheckGameEnd();

            if (result != GameResult.None)
                OnGameEnded(result);
        }

        public void OnGameEnded(GameResult result)
        {
            Console.WriteLine($"Game ended: {result}");
        }

        // ---------------- MAIN CHECK ----------------

        private GameResult CheckGameEnd()
        {
            Color player = board.CurrentPlayer;
            Color opponent = player == Color.White ? Color.Black : Color.White;

            if (IsCheckmate(player))
                return opponent == Color.White
                    ? GameResult.WhiteWins
                    : GameResult.BlackWins;

            if (IsStalemate(player))
                return GameResult.DrawStalemate;

            if (IsInsufficientMaterial())
                return GameResult.DrawInsufficientMaterial;

            if (IsThreefold())
                return GameResult.DrawThreefold;

            if (halfMoveClock >= 100)
                return GameResult.DrawFiftyMove;

            return GameResult.None;
        }

        // ---------------- HISTORY ----------------

        private void UpdateHalfMoveClock(MoveCommand cmd)
        {
            if (cmd.IsCapture || cmd.IsPawnMove)
                halfMoveClock = 0;
            else
                halfMoveClock++;
        }

        private void UpdateHistory()
        {
            positionHistory.Add(board.GetPositionKey());
        }

        private bool IsThreefold()
        {
            string current = board.GetPositionKey();

            return positionHistory
                .Count(p => p == current) >= 3;
        }

        // ---------------- CHECK / MATE ----------------

        private bool IsCheckmate(Color color)
        {
            if (!IsKingInCheck(color))
                return false;

            return !HasLegalMoves(color);
        }

        private bool IsStalemate(Color color)
        {
            if (IsKingInCheck(color))
                return false;

            return !HasLegalMoves(color);
        }

        private bool HasLegalMoves(Color color)
        {
            // Uzimamo kopiju figura da bi izbegli "Collection was modified"
            var playerPieces = board.GetPieces(color).ToList();

            foreach (var piece in playerPieces)
            {
                var availableMoves = piece.GetAvailableMoves(board).ToList();

                foreach (var move in availableMoves)
                {
                    // IsLegalMove unutar sebe radi Execute() i Undo()
                    // To menja board.Pieces, zato su nam potrebni .ToList() gore
                    if (board.IsLegalMove(piece, move))
                        return true;
                }
            }

            return false;
        }

        private bool IsKingInCheck(Color color)
        {
            var king = board.GetKing(color);
            if (king == null) return false;

            foreach (var p in board.GetEnemyPieces(color))
            {
                if (p.GetAvailableMoves(board)
                     .Contains(king.Position))
                    return true;
            }

            return false;
        }

        // ---------------- MATERIAL ----------------

        private bool IsInsufficientMaterial()
        {
            var pieces = board.Pieces;

            if (pieces.All(p => p.Type == PieceType.King))
                return true;

            if (pieces.Count == 3)
            {
                return pieces.Any(p =>
                    p.Type == PieceType.Bishop ||
                    p.Type == PieceType.Knight);
            }

            return false;
        }
    }
}
