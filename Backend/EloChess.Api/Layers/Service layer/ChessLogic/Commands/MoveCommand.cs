using ChessLogic.Pieces;
using ChessLogic.Moves;

namespace ChessLogic.Commands
{
    public class MoveCommand
    {
        private readonly Board board;
        private readonly Piece piece;
        private readonly Position target;

        private Piece? capturedPiece = null;
        private bool isEnPassant = false;
        private bool isCastling = false;
        private Piece? promotedPiece = null;

        private Position previousPosition;

        public MoveCommand(Board board, Piece piece, Position target)
        {
            this.board = board;
            this.piece = piece;
            this.target = target;

            // cuvamo prethodnu poziciju za undo
            previousPosition = piece.Position;
        }

       public void Execute()
        {
            // 1. Resetovanje En Passant statusa
            foreach (var p in board.GetPieces(piece.Color).OfType<Pawn>())
                p.EnPassantEligible = false;

            // 2. Detekcija i uklanjanje žrtve
            capturedPiece = board.GetPiece(target);
            
            // Provera za En Passant
            if (piece is Pawn pawn && target.File != previousPosition.File && capturedPiece == null)
            {
                int captureRank = previousPosition.Rank; // Rank gde stoji protivnički pion
                capturedPiece = board.GetPiece(new Position(captureRank, target.File));
                isEnPassant = true;
            }

            if (capturedPiece != null)
            {
                board.RemovePiece(capturedPiece);
            }

            // 3. Rokada (Mora se proveriti PRE nego što pomerimo kralja na metu)
            if (piece.Type == PieceType.King && Math.Abs(target.File - previousPosition.File) == 2)
            {
                isCastling = true;
                PerformCastling();
            }

            // 4. KLJUČNO: Zvanično pomeranje figure na novu poziciju
            piece.Position = target;
            piece.HasMoved = true;

            // 5. Promocija
            if (piece.Type == PieceType.Pawn && (target.Rank == 0 || target.Rank == 7))
            {
                PromotePawn();
            }
        }

        public void Undo()
        {
            // undo promocije
            if (promotedPiece != null)
            {
                board.RemovePiece(promotedPiece);
                board.AddPiece(piece);
                promotedPiece = null;
            }

            // undo pomeranja
            piece.Position = previousPosition;
            piece.HasMoved = false;

            // undo hvatanja
            if (capturedPiece != null)
            {
                board.AddPiece(capturedPiece);
                capturedPiece = null;
            }

            // undo rokade
            if (isCastling)
            {
                UndoCastling();
                isCastling = false;
            }

            // undo en passant
            if (isEnPassant)
            {
                isEnPassant = false;
            }
        }

        private void PerformCastling()
        {
            int rank = piece.Position.Rank;
            if (target.File == 6) // kratka rokada
            {
                var rook = board.GetPiece(new Position(rank, 7));
                if (rook != null) // Provera da utišamo warning
                {
                    rook.Position = new Position(rank, 5);
                    rook.HasMoved = true;
                }
            }
            else if (target.File == 2) // duga rokada
            {
                var rook = board.GetPiece(new Position(rank, 0));
                if (rook != null)
                {
                    rook.Position = new Position(rank, 3);
                    rook.HasMoved = true;
                }
            }
        }

        // Slično uradi i u UndoCastling... koristimo '!' ako smo sigurni da je rook tamo
        private void UndoCastling()
        {
            int rank = piece.Position.Rank;
            if (target.File == 6)
            {
                var rook = board.GetPiece(new Position(rank, 5))!; 
                rook.Position = new Position(rank, 7);
                rook.HasMoved = false;
            }
            else if (target.File == 2)
            {
                var rook = board.GetPiece(new Position(rank, 3))!;
                rook.Position = new Position(rank, 0);
                rook.HasMoved = false;
            }
        }

        private void PromotePawn(PieceType promoteTo = PieceType.Queen)
        {
            board.RemovePiece(piece);

            switch (promoteTo)
            {
                case PieceType.Queen:
                    promotedPiece = new Queen(piece.Color, target);
                    break;
                case PieceType.Rook:
                    promotedPiece = new Rook(piece.Color, target);
                    break;
                case PieceType.Bishop:
                    promotedPiece = new Bishop(piece.Color, target);
                    break;
                case PieceType.Knight:
                    promotedPiece = new Knight(piece.Color, target);
                    break;
            }

            board.AddPiece(promotedPiece);
        }

        public bool IsCapture
        {
            get { return capturedPiece != null; }
        }

        public bool IsPawnMove
        {
            get { return piece.Type == PieceType.Pawn; }
        }
    }
}



