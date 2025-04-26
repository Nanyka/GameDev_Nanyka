#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public class GameState // Optional: Implement ICloneable if GameState itself needs cloning
    {
        public Board Board { get; }
        public Player NextPlayer { get; }

        private Move? LastMove { get; }

        public Dictionary<Player, Inventory> PlayerInventories { get; }


        // Need a constructor that takes all necessary components
        // Expects Inventory objects that implement ICloneable
        public GameState(Board board, Player nextPlayer, Move? lastMove,
            Dictionary<Player, Inventory> playerInventories)
        {
            if (playerInventories == null) throw new ArgumentNullException(nameof(playerInventories));
            if (!playerInventories.ContainsKey(Player.X) || !playerInventories.ContainsKey(Player.O))
                throw new ArgumentException("PlayerInventories must contain entries for both Player.X and Player.O");

            // Board must implement ICloneable if you intend to clone GameState later
            // if (!(board is ICloneable)) throw new ArgumentException("Board must implement ICloneable");


            Board = board ?? throw new ArgumentNullException(nameof(board)); // Store the provided board (assumed to be a new or cloned instance)
            NextPlayer = nextPlayer;
            LastMove = lastMove;

            // Create a new dictionary for inventories, cloning each Inventory object
            PlayerInventories = new Dictionary<Player, Inventory>();
            foreach (var entry in playerInventories)
            {
                if (entry.Value == null)
                {
                    throw new ArgumentException(
                        $"Inventory object for Player {entry.Key} does not implement ICloneable.");
                }

                // Clone the Inventory object and add to the new dictionary
                PlayerInventories[entry.Key] = (Inventory)((ICloneable)entry.Value).Clone();
            }
        }

        // --- Factory method to start a new game ---
        public static GameState NewGame()
        {
            Board initialBoard = new Board(); // Start with a new board
            Dictionary<Player, Inventory> initialInventories = new Dictionary<Player, Inventory>
            {
                { Player.X, new Inventory() }, // New default Inventory (2,2,2)
                { Player.O, new Inventory() } // New default Inventory (2,2,2)
            };
            // Start with Player.X, no last move
            // The constructor will clone the initial Inventory objects
            return new GameState(initialBoard, Player.X, null, initialInventories);
        }

        /// <summary>
        /// Applies the given move, assuming it's valid.
        /// Returns a new GameState representing the state after the move.
        /// Raises IllegalMoveError if the move is invalid.
        /// </summary>
        /// <param name="move">The move to apply.</param>
        /// <returns>A new GameState object.</returns>
        public GameState ApplyMove(Move move)
        {
            if (IsOver())
            {
                throw new IllegalMoveError("Cannot apply move, game is already over.");
            }

            if (!IsValidMove(move))
            {
                throw new IllegalMoveError($"Move {move.Point} by {NextPlayer} is invalid.");
            }

            Point pieceToPlay = move.Point;
            int strengthToPlay = pieceToPlay.Strength;

            // 1. Create deep copies for the next game state using ICloneable
            Board nextBoard = (Board)Board.Clone(); // Clone the current board
            // Clone each Inventory object in the dictionary
            Dictionary<Player, Inventory> nextInventories = PlayerInventories.ToDictionary(
                entry => entry.Key, // Copy the Player key (value type)
                entry => (Inventory)((ICloneable)entry.Value).Clone() // Clone the Inventory object
            );

            // 2. Update current player's inventory (deduct the piece being played)
            Inventory currentInventory = nextInventories[NextPlayer];
            currentInventory.Deduct(strengthToPlay);

            // 3. Determine if a piece is being replaced and handle board/inventory updates
            int targetRow = pieceToPlay.Row;
            int targetCol = pieceToPlay.Col;

            Player? occupantPlayer = Board.GetPlayerAtCoord(targetRow, targetCol); // Check original board

            if (occupantPlayer.HasValue && occupantPlayer.Value != NextPlayer)
            {
                var oldPointKeyToRemove = Board.GetPointAtCoord(targetRow, targetCol);

                if (oldPointKeyToRemove.HasValue)
                {
                    // Remove the old piece from the *copied* board's grid
                    nextBoard.Remove(oldPointKeyToRemove.Value);

                    // Optional: Return the removed piece to the opponent's inventory
                    // If this is a rule in your game.
                    // nextInventories[occupantPlayer.Value].Add(strengthToReturn);
                }
                else
                {
                    Debug.LogWarning(
                        $"GameState: Found opponent at ({targetRow},{targetCol}) but could not find corresponding Point key on the original board.");
                }
            }

            // 4. Place the new piece on the copied board
            nextBoard.Place(NextPlayer, pieceToPlay);

            // 5. Return the new GameState object
            return new GameState(nextBoard, NextPlayer.Other(), move, nextInventories);
        }

        public Inventory GetInventory(Player player)
        {
            if (!PlayerInventories.TryGetValue(player, out Inventory inventory))
            {
                throw new ArgumentException($"Player {player} not found in inventories dictionary.");
            }

            return inventory;
        }

        public bool IsValidMove(Move move)
        {
            if (IsOver()) return false;

            Point pieceToPlay = move.Point;
            int strengthToPlay = pieceToPlay.Strength;

            Inventory playerInventory = GetInventory(NextPlayer);
            if (!playerInventory.Enough(strengthToPlay)) return false;

            if (!Board.IsOnGrid(pieceToPlay)) return false;

            int targetRow = pieceToPlay.Row;
            int targetCol = pieceToPlay.Col;
            Player? occupantPlayer = Board.GetPlayerAtCoord(targetRow, targetCol);

            if (!occupantPlayer.HasValue) return true; // Empty square
            if (occupantPlayer.Value == NextPlayer) return false; // Own piece

            // Occupied by opponent
            int occupantStrength = Board.GetStrengthAtCoord(targetRow, targetCol);
            return strengthToPlay > occupantStrength;
        }

        public List<Move> LegalMoves()
        {
            // if (IsOver()) return new List<Move>();

            List<Move> moves = new List<Move>();
            Inventory currentInventory = GetInventory(NextPlayer);

            for (int strength = 1; strength <= GameConstants.NumStrengths; strength++)
            {
                if (currentInventory.Enough(strength))
                {
                    for (int row = 1; row <= GameConstants.BoardSize; row++)
                    {
                        for (int col = 1; col <= GameConstants.BoardSize; col++)
                        {
                            Point point = new Point(row, col, strength);
                            Move move = new Move(point);
                            if (IsValidMove(move))
                            {
                                moves.Add(move);
                            }
                        }
                    }
                }
            }

            return moves;
        }

        public bool IsOver()
        {
            if (Has3InARow(Player.X) || Has3InARow(Player.O)) return true;

            Inventory currentPlayerInventory = GetInventory(NextPlayer);
            if (!currentPlayerInventory.GetValidInventoryStrengths().Any()) return true;

            int strEnoughCount = 0;
            foreach (int row in GameConstants.Rows)
            {
                foreach (int col in GameConstants.Cols)
                {
                    Player? playerAtPoint = Board.GetPlayerAtCoord(row, col);
                    int strengthAtPoint = Board.GetStrengthAtCoord(row, col);
                    if (!playerAtPoint.HasValue || playerAtPoint.Value != NextPlayer)
                    {
                        if (!playerAtPoint.HasValue)
                        {
                            strEnoughCount++; 
                        }
                        else 
                        {
                            bool canReplace = false;
                             List<int> availableStrengths = currentPlayerInventory.GetValidInventoryStrengths();
                             foreach (int availableStr in availableStrengths)
                             {
                                 if (availableStr > strengthAtPoint)
                                 {
                                     canReplace = true; 
                                     break; 
                                 }
                             }

                             if (canReplace)
                             {
                                 strEnoughCount++; 
                             }
                        }
                    }
                }
            }
            
            if (strEnoughCount == 0)
            {
                // Debug.Log("Game Over: No legal moves possible for the current player."); // Optional debug
                return true;
            }

            return false;
        }

        private bool Has3InARow(Player player)
        {
            for (int col = 1; col <= GameConstants.BoardSize; col++)
            {
                bool threeInCol = true;
                for (int row = 1; row <= GameConstants.BoardSize; row++)
                {
                    if (Board.GetPlayerAtCoord(row, col) != player)
                    {
                        threeInCol = false;
                        break;
                    }
                }

                if (threeInCol) return true;
            }

            for (int row = 1; row <= GameConstants.BoardSize; row++)
            {
                bool threeInRow = true;
                for (int col = 1; col <= GameConstants.BoardSize; col++)
                {
                    if (Board.GetPlayerAtCoord(row, col) != player)
                    {
                        threeInRow = false;
                        break;
                    }
                }

                if (threeInRow) return true;
            }

            bool threeInDiag1 = true;
            foreach (var point in GameConstants.Diag1)
            {
                if (Board.GetPlayerAtCoord(point.Row, point.Col) != player)
                {
                    threeInDiag1 = false;
                    break;
                }
            }

            if (threeInDiag1) return true;

            bool threeInDiag2 = true;
            foreach (var point in GameConstants.Diag2)
            {
                if (Board.GetPlayerAtCoord(point.Row, point.Col) != player)
                {
                    threeInDiag2 = false;
                    break;
                }
            }

            if (threeInDiag2) return true;

            return false;
        }

        public Player? Winner()
        {
            if (Has3InARow(Player.X)) return Player.X;
            if (Has3InARow(Player.O)) return Player.O;
            return null;
        }

        public override string ToString()
        {
            string boardString = Board.ToString();

            return $"Board:\n{boardString}\n" +
                   $"Next Player: {NextPlayer}\n" +
                   $"Last Move: {LastMove?.Point.ToString()}\n" +
                   $"Inventory of {Player.X}: {PlayerInventories[Player.X]}\n" +
                   $"Inventory of {Player.O}: {PlayerInventories[Player.O]}";
        }
    }
}