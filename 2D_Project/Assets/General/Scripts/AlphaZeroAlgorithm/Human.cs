using System;
using System.Threading.Tasks;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public class Human : IAgent
    {
        // Constants matching the Python code
        private const string ColNames = "ABC";
        private const string ResignAction = "Resign";

        /// <summary>
        /// Parses a string input (like "A11", "B23", "Resign") and converts it into a Point object or null for Resign.
        /// Input format: ColumnLetterRowNumberStrengthNumber (e.g., "A11" -> Col A, Row 1, Strength 1).
        /// Expects 1-based row and strength numbers.
        /// </summary>
        /// <param name="text">The input string from the human player.</param>
        /// <returns>A Point struct if the input is a valid play coordinate, otherwise null.</returns>
        private static Point? PointFromCoords(string text) // Use nullable Point? for None
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Debug.LogWarning("Input is empty.");
                return null;
            }

            // Case-insensitive comparison for "Resign"
            if (text.Trim().Equals(ResignAction, StringComparison.OrdinalIgnoreCase))
            {
                return null; // Indicate Resign by returning null
            }

            // Expected format: Col[A-C]Row[1-3]Strength[1-3] -> e.g., A11, B23, C31
            // Input string length should be at least 3
            if (text.Length < 3)
            {
                Debug.LogWarning($"Invalid input format: {text}. Expected like 'A11' or 'Resign'.");
                return null;
            }

            try
            {
                // Parse column letter (first character)
                char colChar = char.ToUpper(text[0]); // Convert to uppercase for case-insensitivity
                int colIndex = ColNames.IndexOf(colChar); // 0-based index

                if (colIndex == -1)
                {
                    Debug.LogWarning($"Invalid column letter: {text[0]}. Expected A, B, or C.");
                    return null;
                }
                int col1Based = colIndex + 1; // Convert to 1-based column

                // Parse row number (second character)
                if (!int.TryParse(text[1].ToString(), out int row1Based))
                {
                    Debug.LogWarning($"Invalid row number: {text[1]}. Expected 1, 2, or 3.");
                    return null;
                }
                if (row1Based < 1 || row1Based > GameConstants.BoardSize) // Use GameConstants.BoardSize
                {
                    Debug.LogWarning($"Row number out of range: {row1Based}. Expected 1 to {GameConstants.BoardSize}.");
                    return null;
                }


                // Parse strength number (third character)
                if (!int.TryParse(text[2].ToString(), out int strength1Based))
                {
                    Debug.LogWarning($"Invalid strength number: {text[2]}. Expected 1, 2, or 3.");
                    return null;
                }
                if (strength1Based < 1 || strength1Based > GameConstants.NumStrengths) // Use GameConstants.NumStrengths
                {
                    Debug.LogWarning($"Strength number out of range: {strength1Based}. Expected 1 to {GameConstants.NumStrengths}.");
                    return null;
                }

                // If parsing is successful, create and return the Point (1-based)
                return new Point(row1Based, col1Based, strength1Based);
            }
            catch (Exception ex)
            {
                // Catch any other potential parsing errors
                Debug.LogError($"Error parsing input '{text}': {ex.Message}");
                return null;
            }
        }


        // Implementation of the IAgent interface method
        public Task<Move> SelectMove(GameState gameState)
        {
            // Debug.Log("Wait for human playing from input...");
            return Task.FromResult<Move>(null);
        }

        public Move GetMoveFromInput(string inputString)
        {
             Point? point = PointFromCoords(inputString.Trim());
             if (point.HasValue)
             {
                 return new Move(point.Value);
             }
             else
             {
                 // Handle invalid format vs explicit resign if needed,
                 // For simplicity, let's treat any non-coordinate as resign here.
                 Debug.LogWarning($"Input '{inputString}' not recognized as a valid coordinate. Assuming Resign.");
                 return Move.Resign;
             }
        }
    }
}