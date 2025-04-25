using System.Collections.Generic;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public static class GameSetup
    {
        /// <summary>
        /// Creates an initial GameState based on a specified board setup.
        /// </summary>
        /// <param name="boardSetup">A dictionary mapping Point objects (position and strength) to Player.</param>
        /// <param name="startingPlayer">The player who starts the game.</param>
        /// <returns>An initial GameState object.</returns>
        public static GameState SetupState(Dictionary<Point, Player> boardSetup, Player startingPlayer)
        {
            Board initialBoard = new Board(); // Create a new empty board

            // Place the pieces specified in the setup dictionary
            foreach (var entry in boardSetup)
            {
                Point point = entry.Key;
                Player player = entry.Value;

                // Add validation here to ensure points are valid and on the grid
                if (Board.IsOnGrid(point))
                {
                    // Directly add to the internal grid for initial setup.
                    // We bypass the standard Board.Place method which might have assertions
                    // intended for applying moves during gameplay.
                    initialBoard.GetGrid()[point] = player;
                }
                else
                {
                    Debug.LogWarning($"GameSetup: Skipping placing piece {point} for player {player} as it's off grid or invalid.");
                }
            }

            // Create initial inventories (assuming starting with 2 of each for each player)
            // These will be cloned by the GameState constructor.
            Dictionary<Player, Inventory> initialInventories = new Dictionary<Player, Inventory>
            {
                { Player.X, new Inventory() }, // Default constructor initializes with (2,2,2) for strengths 1,2,3
                { Player.O, new Inventory() }
            };

            // Create the initial GameState
            // Last move is null for the starting state
            return new GameState(initialBoard, startingPlayer, null, initialInventories);
        }

        /// <summary>
        /// Creates a new game state with an empty board and Player.X as the starting player.
        /// </summary>
        /// <returns>A new GameState object for the start of a game.</returns>
        public static GameState SetupNewGame()
        {
            // Call SetupState with an empty board dictionary and Player.X as the first player
            return SetupState(new Dictionary<Point, Player>(), Player.X);
        }
    }
}