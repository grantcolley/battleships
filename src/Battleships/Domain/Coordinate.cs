using System;

namespace Battleships.Domain
{
    public readonly struct Coordinate : IEquatable<Coordinate>
    {
        public int X { get; }
        public int Y { get; }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static bool TryParse(string? s, out Coordinate coord)
        {
            coord = default;
            if (string.IsNullOrWhiteSpace(s)) return false;
            s = s.Trim();
            if (s.Length < 2 || s.Length > 3) return false;
            char col = char.ToUpperInvariant(s[0]);
            if (col < 'A' || col > 'J') return false;
            if (!int.TryParse(s.Substring(1), out var row)) return false;
            if (row < 1 || row > 10) return false;
            coord = new Coordinate(col - 'A', row - 1);
            return true;
        }

        public override bool Equals(object? obj) => obj is Coordinate c && Equals(c);
        public bool Equals(Coordinate other) => X == other.X && Y == other.Y;
        public override int GetHashCode() => HashCode.Combine(X, Y);
        public override string ToString() => $"{(char)('A' + X)}{Y + 1}";
    }
}
