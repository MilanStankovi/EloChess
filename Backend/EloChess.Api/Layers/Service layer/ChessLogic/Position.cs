namespace ChessLogic
{
    public class Position
    {
        public int Rank { get; } // X koordinata (0–7)
        public int File { get; } // Y koordinata (0–7)

        public Position(int rank, int file)
        {
            if(rank < 0 || rank > 7 || file < 0 || file > 7)
                throw new ArgumentOutOfRangeException();

            Rank = rank;
            File = file;
        }

        public bool Equals(Position other)
        {
            if(other == null)
                return false;

            return Rank == other.Rank && File == other.File;
        }

        public override bool Equals(object? obj)
        {
            if(obj is Position other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Rank, File);
        }

        public static bool operator ==(Position left, Position right)
        {
            if(left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(Position left, Position right)
        {
            return !(left == right);
        }
    }
}