using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Sentis;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public class Encoder
    {
        // Constants matching the Python encoder
        public const int BoardSize = 3;
        public const int NumStrengths = 3;
        // Plane Definitions (Relative to next_player)
        // Plane 0-2: Current player's piece (Strength 1, 2, 3)
        // Plane 3-5: Opponent's piece (Strength 1, 2, 3)
        // Plane 6: Occupied squares
        public const int NumBoardPlanes = 7;
        public const float MaxInventoryCount = 2.0f; // For normalizing inventory counts (float)
        public const int NumInventoryFeatures = 6; // 3 counts/player * 2 players

        // Constructor
        public Encoder()
        {
            // No specific initialization needed for this static-like class
        }

        /// <summary>
        /// Encodes the current GameState into the board and inventory input tensors for the ONNX model.
        /// </summary>
        /// <param name="gameState">The current GameState object.</param>
        /// <returns>A tuple containing the board TensorFloat (1, 7, 3, 3) and the inventory TensorFloat (1, 6).</returns>
        public (TensorFloat boardTensor, TensorFloat inventoryTensor) Encode(GameState gameState)
        {
            int batchSize = 1; // Assuming batch size 1 for a single game state

            // --- Encode Board Tensor (1, 7, 3, 3) ---
            int boardFlatSize = batchSize * NumBoardPlanes * BoardSize * BoardSize;
            var boardData = new NativeArray<float>(boardFlatSize, Allocator.Temp);

            Player nextPlayer = gameState.NextPlayer;
            Player opponentPlayer = nextPlayer.Other();

            // Keep track of occupied positions to fill plane 6 later
            bool[,] occupied = new bool[BoardSize, BoardSize];

            // Iterate through the pieces currently on the board from the GameState's Board grid
            // The Board's grid keys are Point objects (1-based row, col, strength)
            foreach (var entry in gameState.Board.GetGrid())
            {
                Point pointOnBoard = entry.Key;
                Player playerOnSquare = entry.Value;

                // Convert 1-based coordinates to 0-based for tensor indexing
                int row0Based = pointOnBoard.Row - 1;
                int col0Based = pointOnBoard.Col - 1;
                int strength1Based = pointOnBoard.Strength; // 1, 2, or 3

                int planeIdx;
                if (playerOnSquare == nextPlayer)
                {
                    // Current player's piece: Planes 0, 1, 2 (mapping strength 1->plane 0, 2->plane 1, 3->plane 2)
                    planeIdx = strength1Based - 1;
                }
                else // Opponent's piece
                {
                    // Opponent's piece: Planes 3, 4, 5 (mapping strength 1->plane 3, 2->plane 4, 3->plane 5)
                    planeIdx = (strength1Based - 1) + NumStrengths; // Add 3 to the 0-based strength index
                }

                // Calculate the flat index for this position in the specific player/strength plane
                int index = (0 * NumBoardPlanes * BoardSize * BoardSize) + // Batch index (always 0)
                            (planeIdx * BoardSize * BoardSize) +          // Plane index
                            (row0Based * BoardSize) +                     // Row index
                            col0Based;                                    // Column index

                boardData[index] = 1.0f; // Mark this position in the relevant plane

                // Mark this position as occupied by any piece
                occupied[row0Based, col0Based] = true;
            }

            // Fill Plane 6 (Occupied squares)
            int occupiedPlane = 6;
            for (int r = 0; r < BoardSize; r++) // 0-based iteration
            {
                for (int c = 0; c < BoardSize; c++) // 0-based iteration
                {
                    if (occupied[r, c])
                    {
                        int index = (0 * NumBoardPlanes * BoardSize * BoardSize) + // Batch index
                                    (occupiedPlane * BoardSize * BoardSize) +   // Occupied plane index
                                    (r * BoardSize) +                        // Row index
                                    c;                                       // Column index
                        boardData[index] = 1.0f; // Mark this position as occupied
                    }
                }
            }

            // Create the board TensorFloat
            TensorFloat boardTensor = new TensorFloat(new TensorShape(batchSize, NumBoardPlanes, BoardSize, BoardSize), boardData);
            boardData.Dispose(); // Dispose the NativeArray

            // --- Encode Inventory Tensor (1, 6) ---
            var inventoryData = new NativeArray<float>(batchSize * NumInventoryFeatures, Allocator.Temp);

            Inventory currentPlayerInventory = gameState.GetInventory(nextPlayer);
            Inventory opponentPlayerInventory = gameState.GetInventory(opponentPlayer);

            // Populate current player's inventory counts (Indices 0-2)
            for (int strength1Based = 1; strength1Based <= NumStrengths; strength1Based++) // Strengths 1, 2, 3
            {
                int strength0BasedIndex = strength1Based - 1; // 0, 1, or 2
                int count = currentPlayerInventory.GetCount(strength1Based);
                // Normalize and assign
                inventoryData[strength0BasedIndex] = count / MaxInventoryCount;
            }

            // Populate opponent's inventory counts (Indices 3-5)
            for (int strength1Based = 1; strength1Based <= NumStrengths; strength1Based++) // Strengths 1, 2, 3
            {
                int strength0BasedIndex = strength1Based - 1; // 0, 1, or 2
                int count = opponentPlayerInventory.GetCount(strength1Based);
                // Normalize and assign
                inventoryData[strength0BasedIndex + NumStrengths] = count / MaxInventoryCount; // Add 3 to the index
            }

            // Create the inventory TensorFloat
            TensorFloat inventoryTensor = new TensorFloat(new TensorShape(batchSize, NumInventoryFeatures), inventoryData);
            inventoryData.Dispose(); // Dispose the NativeArray

            return (boardTensor, inventoryTensor);
        }


        /// <summary>
        /// Encodes a Move object into a single integer index (0-26).
        /// Assumes move index is (row-1)*9 + (col-1)*3 + (strength-1) for 1-based row/col/strength.
        /// </summary>
        /// <param name="move">The Move object to encode.</param>
        /// <returns>The integer index representing the move (0-26).</returns>
        /// <exception cref="ArgumentException">Thrown if the move is invalid or off grid.</exception>
        public int EncodeMove(Move move)
        {
            if (move == null || !move.IsPlay)
            {
                throw new ArgumentException($"Cannot encode invalid or non-play move: {move}");
            }

            Point point = move.Point;
            int row = point.Row;
            int col = point.Col;
            int strength = point.Strength;

            if (row < 1 || row > BoardSize || col < 1 || col > BoardSize || strength < 1 || strength > NumStrengths)
            {
                throw new ArgumentException($"Move contains invalid coordinates or strength: {move}");
            }

            // Calculate the index (0-based)
            // Index = (row - 1) * BoardSize * NumStrengths + (col - 1) * NumStrengths + (strength - 1)
            int index = (row - 1) * BoardSize * NumStrengths + (col - 1) * NumStrengths + (strength - 1);

            return index;
        }

        /// <summary>
        /// Decodes an integer action index (0-26) back into a Move object.
        /// Assumes index corresponds to (row-1)*9 + (col-1)*3 + (strength-1).
        /// </summary>
        /// <param name="index">The integer index (0-26) from the model's action output.</param>
        /// <returns>A Move object representing the decoded action.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is out of the valid range.</exception>
        public Move DecodeMoveIndex(int index)
        {
            int totalMoves = BoardSize * BoardSize * NumStrengths; // 27
            if (index < 0 || index >= totalMoves)
            {
                throw new ArgumentOutOfRangeException(nameof(index), $"Invalid move index: {index}. Must be 0 to {totalMoves - 1}.");
            }

            // Reverse the encoding calculation to get 0-based strength, col, row
            int strength0Based = index % NumStrengths;
            int remaining = index / NumStrengths;
            int col0Based = remaining % BoardSize;
            int row0Based = remaining / BoardSize;

            // Convert back to 1-based for Point
            int row1Based = row0Based + 1;
            int col1Based = col0Based + 1;
            int strength1Based = strength0Based + 1;

            // Create the Point and Move objects
            Point point = new Point(row1Based, col1Based, strength1Based);
            Move move = new Move(point);

            return move;
        }

        /// <summary>
        /// Decodes the board tensor and inventory vector back into a GameState object.
        /// NOTE: This attempts to reconstruct the game state from the tensor data
        /// based on the encoding logic. Some information like LastMove is not recoverable.
        /// </summary>
        /// <param name="boardTensor">The board TensorFloat (1, 7, 3, 3).</param>
        /// <param name="inventoryTensor">The inventory TensorFloat (1, 6).</param>
        /// <returns>A reconstructed GameState object.</returns>
        /// <exception cref="ArgumentException">Thrown if tensor shapes are incorrect.</exception>
        public GameState Decode(TensorFloat boardTensor, TensorFloat inventoryTensor)
        {
            // Validate tensor shapes
            if (boardTensor.shape.length != 4 || boardTensor.shape[1] != NumBoardPlanes || boardTensor.shape[2] != BoardSize || boardTensor.shape[3] != BoardSize)
            {
                throw new ArgumentException($"Invalid board_tensor shape: {boardTensor.shape}. Expected (1, {NumBoardPlanes}, {BoardSize}, {BoardSize}).");
            }
            if (inventoryTensor.shape.length != 2 || inventoryTensor.shape[1] != NumInventoryFeatures)
            {
                throw new ArgumentException($"Invalid inventory_tensor shape: {inventoryTensor.shape}. Expected (1, {NumInventoryFeatures}).");
            }

            // Ensure tensors are readable if they were on the GPU
            // In a real async scenario, you'd need to await ReadbackRequestAsync() and call MakeReadable()
            // before accessing the data via ToNativeArray(). Assuming they are readable here for demonstration.
            boardTensor.MakeReadable();
            inventoryTensor.MakeReadable();

            // Get data as NativeArrays
            float[] boardData = boardTensor.ToReadOnlyArray();
            float[] inventoryData = inventoryTensor.ToReadOnlyArray();


            // 1. Determine Next Player from Plane 6 (Replicating Python's potential logic inconsistency)
            // Python uses board_tensor[6][0][0]. We'll use the flat index for (batch 0, plane 6, row 0, col 0).
            int plane6Index_r0_c0 = (0 * NumBoardPlanes * BoardSize * BoardSize) + (6 * BoardSize * BoardSize) + (0 * BoardSize) + 0;
            // Using >= 0.5 for robustness against potential float inaccuracies
            Player nextPlayer = (boardData[plane6Index_r0_c0] >= 0.5f) ? Player.X : Player.O;
            Player opponentPlayer = nextPlayer.Other();


            // 2. Reconstruct Inventories
            Dictionary<int, int> xInvDict = new Dictionary<int, int>();
            Dictionary<int, int> oInvDict = new Dictionary<int, int>();

            // De-normalize counts based on the inventory vector
            for (int strength0Based = 0; strength0Based < NumStrengths; strength0Based++) // 0, 1, 2
            {
                int strength1Based = strength0Based + 1; // 1, 2, 3

                // Current player counts (indices 0-2)
                float normCurrentCount = inventoryData[strength0Based];
                int currentCount = Mathf.RoundToInt(normCurrentCount * MaxInventoryCount);

                // Opponent counts (indices 3-5)
                float normOpponentCount = inventoryData[strength0Based + NumStrengths];
                int opponentCount = Mathf.RoundToInt(normOpponentCount * MaxInventoryCount);

                // Assign counts to correct player's dictionary based on nextPlayer
                if (nextPlayer == Player.X)
                {
                    xInvDict[strength1Based] = currentCount;
                    oInvDict[strength1Based] = opponentCount;
                }
                else // nextPlayer == Player.O
                {
                    oInvDict[strength1Based] = currentCount;
                    xInvDict[strength1Based] = opponentCount;
                }
            }

            // Create Inventory objects (using the C# Inventory class with 1-based keys)
            Dictionary<Player, Inventory> playerInventories = new Dictionary<Player, Inventory>
            {
                { Player.X, new Inventory(xInvDict) },
                { Player.O, new Inventory(oInvDict) }
            };


            // 3. Reconstruct Board
            Board newBoard = new Board();

            // Iterate through board squares (0-based for tensor)
            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    bool foundPiece = false;
                    // Check piece planes (0-5)
                    for (int planeIdx = 0; planeIdx < NumBoardPlanes - 1; planeIdx++) // Exclude occupied plane (plane 6)
                    {
                        // Calculate the flat index for this position in this plane
                        int index = (0 * NumBoardPlanes * BoardSize * BoardSize) +
                                    (planeIdx * BoardSize * BoardSize) +
                                    (r * BoardSize) +
                                    c;

                        // Check if this plane is active at this position (using >= 0.5 for robustness)
                        if (boardData[index] >= 0.5f)
                        {
                            if (foundPiece)
                            {
                                // Warning: Multiple planes active for the same square in the tensor!
                                // This indicates an issue with the encoding or tensor data.
                                Debug.LogWarning($"Decoding conflict at ({r + 1},{c + 1}). Multiple planes active ({planeIdx}). Skipping placement for this square.");
                                // Decide how to handle conflicts (e.g., skip, use first found, error)
                                // Let's break and skip this square based on Python's break logic in conflict
                                foundPiece = false; // Reset found_piece flag to indicate conflict skipped placement
                                break;
                            }

                            // Determine player and strength based on plane_idx and nextPlayer
                            int strength1Based = (planeIdx % NumStrengths) + 1; // Strength 1, 2, or 3
                            bool isCurrentPlayerPiece = planeIdx < NumStrengths; // Planes 0, 1, 2 are current player's

                            Player player = isCurrentPlayerPiece ? nextPlayer : opponentPlayer;

                            // Create Point (using 1-based row, col, strength)
                            Point point = new Point(r + 1, c + 1, strength1Based);

                            // Place the piece on the new board
                            // We bypass Board.Place's assertions because we are reconstructing from tensor
                            newBoard.GetGrid()[point] = player; // Directly add to the internal dictionary
                            foundPiece = true; // Mark that a piece was found for this square
                        }
                    }
                }
            }
            
            // 4. Create GameState
            // We cannot recover the last move made from the tensor state alone
            GameState decodedState = new GameState(newBoard, nextPlayer, null, playerInventories);

            return decodedState;
        }

        /// <summary>
        /// Finds the index of the first move in the list that has the same Point as the target move.
        /// </summary>
        /// <param name="moveList">The list of Move objects to search within.</param>
        /// <param name="targetMove">The target Move object.</param>
        /// <returns>The 0-based index of the first matching move, or -1 if not found.</returns>
        public int FindMoveIndexByPoint(List<Move> moveList, Move targetMove)
        {
            if (moveList == null || targetMove == null) return -1;

            for (int index = 0; index < moveList.Count; index++)
            {
                // Use the Move class's Equals method, which compares Points
                if (moveList[index] == targetMove)
                {
                    return index;
                }
            }
            return -1; // Move not found in the list
        }

        /// <summary>
        /// Returns the total number of possible distinct moves (9 squares * 3 strengths = 27).
        /// </summary>
        /// <returns>The total number of moves.</returns>
        public int NumMoves()
        {
            return BoardSize * BoardSize * NumStrengths; // 27
        }

        /// <summary>
        /// Returns the shape of the board input tensor (7, 3, 3).
        /// </summary>
        /// <returns>A TensorShape representing the board input shape.</returns>
        public TensorShape Shape()
        {
            // Note: The Python shape is (planes, rows, cols). Sentis is (batch, planes, rows, cols).
            // We represent the *model* shape here, which is effectively (7, 3, 3) from the model's perspective,
            // but Sentis will add the batch dimension.
            // Let's return the Sentis TensorShape including the batch for clarity in a Unity context.
            return new TensorShape(1, NumBoardPlanes, BoardSize, BoardSize); // Assuming batch size 1
            // If you literally need the (7, 3, 3) shape as a concept:
            // return new TensorShape(NumBoardPlanes, BoardSize, BoardSize); // This might be less useful for TensorFloat creation
        }

        /// <summary>
        /// Returns the shape of the inventory input tensor (6).
        /// </summary>
        /// <returns>A TensorShape representing the inventory input shape.</returns>
        public TensorShape InventoryShape()
        {
            // Python shape is (6,). Sentis is (batch, features).
            return new TensorShape(1, NumInventoryFeatures); // Assuming batch size 1
        }

        // Helper methods matching Python for clarity (could also be properties)
        public int GetBoardSize() => BoardSize;
        public int GetNumStrengths() => NumStrengths;
        public int GetNumPlanes() => NumBoardPlanes;
        public int GetNumInventoryFeatures() => NumInventoryFeatures;
    }
}