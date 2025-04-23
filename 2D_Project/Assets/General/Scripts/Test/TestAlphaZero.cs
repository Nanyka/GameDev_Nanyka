using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaZeroAlgorithm;
using TMPro;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.UI;
using Human = AlphaZeroAlgorithm.Human;

namespace TheAiAlchemist
{
    public class TestAlphaZero : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextMeshProUGUI gameStateText;
        [SerializeField] private ModelAsset modelAsset;
        
        private IAgent _humanAgent;
        private BotAgent _botAgent;
        private GameState _currentGameState;
        private Dictionary<Player, IAgent> _players;

        private async void Start()
        {
            await Init();
        }

        private async Task Init()
        {
            _humanAgent = new AlphaZeroAlgorithm.Human();
            _botAgent = new BotAgent(modelAsset);
            _players = new Dictionary<Player, IAgent>
            {
                { Player.X, _humanAgent },
                { Player.O, _botAgent }
            };

            Debug.Log("Human vs Human Console Test Game started!");
            _currentGameState = GameSetup.SetupNewGame();
            await StartNextTurn();
        }

        public async void LogInput()
        {
            if (inputField != null)
            {
                Debug.Log("Input: " + inputField.text);
                var move = _humanAgent.GetMoveFromInput(inputField.text);
                await ApplySelectedMove(move);
                inputField.text = "";
            }
            else
            {
                Debug.LogWarning("No InputField assigned to the InputLogger script.");
            }
        }

        private async Task StartNextTurn()
        {
            if (_currentGameState.IsOver())
            {
                EndGame();
                return;
            }
            
            if (gameStateText != null)
                gameStateText.text = _currentGameState.ToString();

            var nextMove = await _players[_currentGameState.NextPlayer].SelectMove(_currentGameState);
            if (nextMove == null)
            {
                Debug.Log("Waiting for human playing...");
            }
            else
                await ApplySelectedMove(nextMove);
        }

        private async Task ApplySelectedMove(Move move)
        {
            Debug.Log($"Apply move: {move}");
            if (_currentGameState.IsOver())
            {
                Debug.LogWarning("Attempted to apply move to a game that is already over.");
                EndGame();
                return;
            }

            try
            {
                _currentGameState = _currentGameState.ApplyMove(move);
                Debug.Log($"Move applied successfully: {move}");
                await StartNextTurn();
            }
            catch (IllegalMoveError ex)
            {
                Debug.LogError($"Illegal move applied by {_currentGameState.NextPlayer}: {ex.Message}. " +
                               $"Game state remains unchanged.");
                if (_currentGameState.NextPlayer != Player.X)
                {
                    Debug.LogError("Bot attempted an illegal move. Game halted due to bot error.");
                    EndGame(); 
                }
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"An unexpected error occurred while applying move {move} for player " +
                    $"{_currentGameState.NextPlayer}: {ex.Message}");
                EndGame(); 
            }
        }

        private void EndGame()
        {
            Debug.Log("--- Game Over ---");
            if (gameStateText != null) gameStateText.text = _currentGameState.ToString();
            // Debug.Log(_currentGameState.ToString()); // Console log
            
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