using System;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public class Board : ICloneable
    {
        private Dictionary<Point, Player> _grid;

        public Board()
        {
            _grid = new Dictionary<Point, Player>();
        }

        // Copy constructor (kept private)
        private Board(Dictionary<Point, Player> grid)
        {
            _grid = new Dictionary<Point, Player>(grid);
        }

        /// <summary>
        /// Creates a deep copy of the board. Implements ICloneable.
        /// </summary>
        /// <returns>An object that is a deep copy of the current instance.</returns>
        public object Clone()
        {
            return new Board(_grid); // Use the copy constructor
        }

        /// <summary>
        /// Places a player's piece (defined by the Point, including strength) on the board.
        /// Assumes the move is valid (checked externally).
        /// </summary>
        /// <param name="player">The player placing the piece.</param>
        /// <param name="point">The Point object defining position and strength.</param>
        public void Place(Player player, Point point)
        {
            _grid[point] = player;
        }

        /// <summary>
        /// Removes a piece at the given point from the board.
        /// </summary>
        /// <param name="point">The Point object defining the position and strength of the piece to remove.</param>
        public void Remove(Point point)
        {
            if (_grid.ContainsKey(point))
            {
                _grid.Remove(point);
            }
            else
            {
                Debug.LogWarning($"Board: Tried to remove Point {point} but it was not found in the grid.");
            }
        }

        public static bool IsOnGrid(Point point)
        {
            return 1 <= point.Row && point.Row <= GameConstants.BoardSize &&
                   1 <= point.Col && point.Col <= GameConstants.BoardSize;
        }

        public Dictionary<Point, Player> GetGrid()
        {
            return _grid;
        }

        public Player? Get(Point point)
        {
            if (_grid.TryGetValue(point, out Player player))
            {
                return player;
            }

            return null;
        }

        public Player? GetPlayerAtCoord(int row, int col)
        {
            foreach (var entry in _grid)
            {
                if (entry.Key.Row == row && entry.Key.Col == col)
                {
                    return entry.Value;
                }
            }

            return null;
        }

        public int GetStrengthAtCoord(int row, int col)
        {
            foreach (var entry in _grid)
            {
                if (entry.Key.Row == row && entry.Key.Col == col)
                {
                    return entry.Key.Strength;
                }
            }

            return 0;
        }

        public Point? GetPointAtCoord(int row, int col)
        {
            foreach (var entry in _grid)
            {
                if (entry.Key.Row == row && entry.Key.Col == col)
                {
                    return entry.Key;
                }
            }

            return null;
        }

        public override string ToString()
        {
            var outputLines = new List<string> { "    A     B    C" };

            for (int row = 1; row <= GameConstants.BoardSize; row++)
            {
                var piecesInRow = new List<string>();
                for (int col = 1; col <= GameConstants.BoardSize; col++)
                {
                    Player? player = GetPlayerAtCoord(row, col);
                    if (player.HasValue)
                    {
                        int strength = GetStrengthAtCoord(row, col);
                        piecesInRow.Add($"{player.Value.ToString().ToUpper()}{strength}");
                    }
                    else
                    {
                        piecesInRow.Add("    ");
                    }
                }

                outputLines.Add($"{row}  {string.Join("| ", piecesInRow)}");
            }

            return string.Join("\n", outputLines);
        }
    }
}