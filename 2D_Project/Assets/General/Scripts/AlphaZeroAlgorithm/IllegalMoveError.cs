using System;

namespace AlphaZeroAlgorithm
{
    public class IllegalMoveError : Exception
    {
        public IllegalMoveError() { }

        public IllegalMoveError(string message) : base(message) { }

        public IllegalMoveError(string message, Exception inner) : base(message, inner) { }
    }
}