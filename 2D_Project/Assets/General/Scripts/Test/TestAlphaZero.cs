using System;
using System.Collections;
using AlphaZeroAlgorithm;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Human = AlphaZeroAlgorithm.Human;

namespace TheAiAlchemist
{
    public class TestAlphaZero : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI gameStateText;
        private IAgent _playerXAgent;
        private IAgent _playerOAgent;
        private GameState _currentGameState;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            _playerXAgent = new AlphaZeroAlgorithm.Human();
            _playerOAgent = new AlphaZeroAlgorithm.Human();
            _currentGameState = GameSetup.SetupNewGame();
            
            Debug.Log("Human vs Human Console Test Game started!");
            Debug.Log(_currentGameState.IsOver());
            StartNextTurn();
        }

        public void LogInput()
        {
            // Check which type of InputField is assigned and log its text
            if (inputField != null)
            {
                Debug.Log("Standard Input: " + inputField.text);
                var move = _playerXAgent.GetMoveFromInput(inputField.text);
                ApplySelectedMove(move);
            }
            else
            {
                Debug.LogWarning("No InputField assigned to the InputLogger script.");
            }
        }
        
        private void StartNextTurn()
        {
            // Check if the game ended from the previous move
            if (_currentGameState.IsOver())
            {
                EndGame();
                return;
            }

            // --- Display Current Game State ---
            // Update the UI Text element to show the board and game info.
            if (gameStateText != null)
            {
                gameStateText.text = _currentGameState.ToString();
            }
            Debug.Log("--- Current Game State ---");
            Debug.Log(_currentGameState.ToString()); 
            Debug.Log("--------------------------");


            // Determine whose turn it is
            if (_currentGameState.NextPlayer == Player.X) // Assuming Player.X is the UI-controlled human player
            {
                 Debug.Log("Human player's turn (Player X). Please enter your move in the UI.");
            }
            else // Assuming Player.O is the bot (or another agent)
            {
                Debug.Log($"Agent's turn ({_currentGameState.NextPlayer}). Calculating move...");
            }
        }
        
        private void ApplySelectedMove(Move move)
        {
            // Check if the game ended before applying the move (should be caught by StartNextTurn)
             if (_currentGameState.IsOver())
            {
                Debug.LogWarning("Attempted to apply move to a game that is already over.");
                EndGame(); // Ensure game ends properly
                return;
            }

            try
            {
                // Apply the move using GameState's ApplyMove (which returns a new state)
                // This method throws IllegalMoveError if the move is invalid according to GameState rules.
                _currentGameState = _currentGameState.ApplyMove(move);
                Debug.Log($"Move applied successfully: {move}");

                // If move was applied successfully, proceed to the next turn
                StartNextTurn(); // This will determine the next player and enable UI or start bot coroutine
            }
            catch (IllegalMoveError ex)
            {
                Debug.LogError($"Illegal move applied by {_currentGameState.NextPlayer}: {ex.Message}. Game state remains unchanged.");
                // If it was the human's turn, StartNextTurn (which is called next) will
                // correctly re-enable the human input UI, allowing them to try again.
                // If it was the bot's turn, this indicates a bug in the bot's legal move generation/selection.
                 if (_currentGameState.NextPlayer != Player.X) // It was bot's turn (assuming Player X is human)
                 {
                     Debug.LogError("Bot attempted an illegal move. Game halted due to bot error.");
                     EndGame(); // Or handle bot errors differently
                 }
                 else
                 {
                     // It was human's turn, illegal move message already logged.
                     // StartNextTurn is called, which will reactive the human input UI.
                 }
            }
            catch (Exception ex)
            {
                 // Handle other unexpected errors during move application
                 Debug.LogError($"An unexpected error occurred while applying move {move} for player {_currentGameState.NextPlayer}: {ex.Message}");
                 EndGame(); // Halt game on unexpected errors
            }
        }
        
        private void EndGame()
        {
            Debug.Log("--- Game Over ---");
            // Display final game state again
            if (gameStateText != null) gameStateText.text = _currentGameState.ToString();
            Debug.Log(_currentGameState.ToString()); // Console log

            // Determine and display the winner
            Player? winner = _currentGameState.Winner();
            if (winner.HasValue)
            {
                if (gameStateText != null) gameStateText.text += $"\nWinner: {winner.Value}";
                Debug.Log($"Winner: {winner.Value}");
            }
            else
            {
                if (gameStateText != null) gameStateText.text += "\nIt's a draw.";
                Debug.Log("It's a draw.");
            }
        }
    }
}