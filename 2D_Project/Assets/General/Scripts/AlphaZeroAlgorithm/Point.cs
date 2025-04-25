using System;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public struct Point : IEquatable<Point>
    {
        // Using PascalCase for public properties
        public int Row { get; }
        public int Col { get; }
        public int Strength { get; } // Using 'Strength' instead of 'Str' for clarity

        public Point(int row, int col, int strength)
        {
            if (row < 1 || col < 1 || strength < 1)
            {
                Debug.LogWarning($"Creating Point with potentially invalid 1-based values: " +
                                 $"Row={row}, Col={col}, Strength={strength}");
                // Depending on your needs, you might want to throw an ArgumentOutOfRangeException here.
            }

            Row = row;
            Col = col;
            Strength = strength;
        }

        // Implementing IEquatable<Point> for value comparison
        public bool Equals(Point other)
        {
            return Row == other.Row && Col == other.Col && Strength == other.Strength;
        }

        public override bool Equals(object obj)
        {
            return obj is Point other && Equals(other);
        }

        public override int GetHashCode()
        {
            // Simple hash code combination for the fields
            unchecked
            {
                int hashCode = Row.GetHashCode();
                hashCode = (hashCode * 397) ^ Col.GetHashCode();
                hashCode = (hashCode * 397) ^ Strength.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Point(row={Row}, col={Col}, strength={Strength})";
        }

        // C# structs are value types and are copied by default.
        // An explicit deep copy implementation like Python's __deepcopy__ is not typically needed
        // unless the struct contained reference types you needed to deep copy (this one doesn't).
    }
}