namespace TheAiAlchemist
{
    using UnityEngine;
    using Unity.Sentis;
    using System.Collections.Generic;
    using Unity.Collections; // Required for NativeArray

    public static class GameEncoder
    {
        // Constants matching the Python encoder
        private const int BoardSize = 3;
        private const int NumStrengths = 3;
        private const int NumBoardPlanes = 7; // 3 curr + 3 opp + 1 occupied
        private const int NumInventoryFeatures = 6; // 3 curr + 3 opp
        private const float MaxInventoryCount = 2.0f; // From Python script

        // Helper to convert a 1D board index (0-8) to 2D (row, col)
        private static (int row, int col) IndexToRowCol(int index)
        {
            if (index < 0 || index >= BoardSize * BoardSize)
            {
                Debug.LogError($"Invalid board index: {index}");
                return (-1, -1); // Indicate error
            }
            int row = index / BoardSize;
            int col = index % BoardSize;
            // Debug.Log($"Circle id {index}: row {row}, col {col}");
            return (row, col);
        }

        /// <summary>
        /// Encodes the current board state into a (1, 7, 3, 3) tensor, relative to the current player.
        /// Planes 0-2: Current player units (Strength 1, 2, 3)
        /// Planes 3-5: Opponent units (Strength 1, 2, 3)
        /// Plane 6: Occupied squares
        /// </summary>
        /// <param name="circlesOnBoard">List of Circle objects currently on the board.</param>
        /// <param name="currentPlayerId">The ID of the player whose turn it is (1 or 2).</param>
        /// <returns>A TensorFloat with shape (1, 7, 3, 3).</returns>
        public static Tensor<float> EncodeBoardState(List<ICircleTrait> circlesOnBoard, int currentPlayerId)
        {
            int batchSize = 1;
            int flatSize = batchSize * NumBoardPlanes * BoardSize * BoardSize;
            var boardData = new NativeArray<float>(flatSize, Allocator.Temp); // Use Temp allocator for short-lived data

            // Keep track of occupied positions to fill plane 6 later
            bool[,] occupied = new bool[BoardSize, BoardSize];

            // Iterate through the pieces currently on the board
            foreach (var circle in circlesOnBoard)
            {
                (int row, int col) = IndexToRowCol(circle.GetId());
                if (row == -1 || col == -1) continue; // Skip if index was invalid

                int piecePlayerId = circle.GetPlayerId();
                int piecePriority = circle.GetPriority(); // Assuming 0, 1, 2 for strengths 1, 2, 3

                int plane;
                if (piecePlayerId == currentPlayerId)
                {
                    // Current player's piece: Planes 0, 1, 2 (mapping priority 0->plane 0, 1->plane 1, 2->plane 2)
                    plane = piecePriority;
                }
                else // Opponent's piece
                {
                    // Opponent's piece: Planes 3, 4, 5 (mapping priority 0->plane 3, 1->plane 4, 2->plane 5)
                    plane = piecePriority + NumStrengths; // Add 3 to the priority index
                }

                // Calculate the flat index for this position in the specific player/strength plane
                int index = (0 * NumBoardPlanes * BoardSize * BoardSize) + // Batch index (always 0 for batchSize 1)
                            (plane * BoardSize * BoardSize) +         // Plane index
                            (row * BoardSize) +                       // Row index
                            col;                                      // Column index

                boardData[index] = 1.0f; // Mark this position in the relevant plane

                // Mark this position as occupied by any piece
                occupied[row, col] = true;
            }

            // Fill Plane 6 (Occupied squares)
            int occupiedPlane = 6;
            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    if (occupied[r, c])
                    {
                         int index = (0 * NumBoardPlanes * BoardSize * BoardSize) + // Batch index
                                     (occupiedPlane * BoardSize * BoardSize) +  // Occupied plane index
                                     (r * BoardSize) +                       // Row index
                                     c;                                      // Column index
                        boardData[index] = 1.0f; // Mark this position as occupied
                    }
                }
            }

            // Create the TensorFloat from the NativeArray
            Tensor<float> boardTensor = new Tensor<float>(new TensorShape(batchSize, NumBoardPlanes, BoardSize, BoardSize), boardData);

            boardData.Dispose(); // Dispose the NativeArray as it's no longer needed

            return boardTensor;
        }

        /// <summary>
        /// Encodes the remaining inventory state into a (1, 6) tensor, relative to the current player,
        /// using the provided InventoryComp instances.
        /// Indices 0-2: Current Player's inventory (Strength 1, 2, 3 counts, normalized)
        /// Indices 3-5: Opponent Player's inventory (Strength 1, 2, 3 counts, normalized)
        /// </summary>
        /// <param name="currentPlayerInventory">The InventoryComp instance for the current player.</param>
        /// <param name="opponentPlayerInventory">The InventoryComp instance for the opponent player.</param>
        /// <returns>A TensorFloat with shape (1, 6).</returns>
        public static Tensor<float> EncodeInventoryState(IInventoryComp currentPlayerInventory, IInventoryComp opponentPlayerInventory)
        {
            int batchSize = 1;
            var inventoryData = new NativeArray<float>(batchSize * NumInventoryFeatures, Allocator.Temp);

            // Get the raw inventory lists from the components
            List<int> currentPlayerItems = currentPlayerInventory.GetItems();
            List<int> opponentPlayerItems = opponentPlayerInventory.GetItems();

            // Ensure lists have enough elements (should be 3 for strengths 0, 1, 2)
            if (currentPlayerItems.Count < NumStrengths || opponentPlayerItems.Count < NumStrengths)
            {
                Debug.LogError("Inventory lists do not contain enough elements for all strengths (expected 3).");
                 // Fill with zeros or handle appropriately if inventory is incomplete
                 // For this example, we'll proceed but access might be out of bounds if counts are wrong
            }


            // Populate current player's inventory counts (Indices 0-2)
            for (int strengthIndex = 0; strengthIndex < NumStrengths; strengthIndex++) // Strengths 0, 1, 2
            {
                 // Access count directly from the list by strengthIndex
                 int count = (strengthIndex < currentPlayerItems.Count) ? currentPlayerItems[strengthIndex] : 0;
                 // Normalize and assign
                 inventoryData[strengthIndex] = count / MaxInventoryCount;
            }

            // Populate opponent's inventory counts (Indices 3-5)
            for (int strengthIndex = 0; strengthIndex < NumStrengths; strengthIndex++) // Strengths 0, 1, 2
            {
                 // Access count directly from the list by strengthIndex
                 int count = (strengthIndex < opponentPlayerItems.Count) ? opponentPlayerItems[strengthIndex] : 0;
                 // Normalize and assign
                 inventoryData[strengthIndex + NumStrengths] = count / MaxInventoryCount; // Add 3 to the index
            }

            Tensor<float> inventoryTensor = new Tensor<float>(new TensorShape(batchSize, NumInventoryFeatures), inventoryData);
            inventoryData.Dispose();
            return inventoryTensor;
        }
    }
}