using System;

namespace AlphaZeroAlgorithm
{
    public class Move : IEquatable<Move>
    {
        // Point defining the position and strength of the piece being played.
        // In this game, moves always involve placing a piece.
        public Point Point { get; }

        // In this game, a Move object always represents a play with a Point
        public bool IsPlay => true;

        // If you needed a Pass or Resign move, you would handle that differently,
        // perhaps with a static Pass property or similar, and adjust the constructor
        // and IsPlay property accordingly.
        // private static readonly Move _pass = new Move(); // Example for a pass move

        private static readonly Move _resign = new Move(); // Use the private constructor

        public static Move Resign => _resign;

        public Move() { }

        public Move(Point point)
        {
            // The Python code had an assert that point is not None.
            // In C#, struct Point cannot be null unless it's a nullable Point?.
            // Since a Move in this game seems to always involve a specific piece placement,
            // we expect a valid Point here.
            Point = point;
            // IsPlay is always true for a Move created this way
        }

        // Private constructor for special moves like Pass (if implemented)
        /*
        private Move()
        {
            Point = default; // Default Point value (might be (0,0,0))
            IsPlay = false;
        }
        */

        // Implementing IEquatable<Move> for object comparison
        public bool Equals(Move other)
        {
            if (other == null) return false;
            // Two moves are equal if their Points are equal (using Point's Equals implementation)
            return Point.Equals(other.Point);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Move);
        }

        public override int GetHashCode()
        {
            // Make Move objects hashable based on their Point's hash code
            return Point.GetHashCode();
        }

        // Operator overloads for convenience
        public static bool operator ==(Move left, Move right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null)) return false;
            return left.Equals(right);
        }

        public static bool operator !=(Move left, Move right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            // If Pass move was implemented:
            // if (!IsPlay) return "Resign";
            return Point.ToString(); // Use Point's ToString()
        }
    }
}