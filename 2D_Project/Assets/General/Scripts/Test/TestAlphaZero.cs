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
        // [SerializeField] private ModelAsset modelAsset;
        [SerializeField] private AddressableManagerSO addressableManager;
        [SerializeField] private string modelAddress;
        
        private IAgent _humanAgent;
        private AlphaZeroAgent _botAgent;
        private GameState _currentGameState;
        private Dictionary<Player, IAgent> _players;

        private async void Start()
        {
            await Init();
        }

        private async Task Init()
        {
            _humanAgent = new AlphaZeroAlgorithm.Human();

            var modelAsset = await addressableManager.GetModel(modelAddress);
            if (modelAsset == null)
            {
                Debug.Log("The game fail to load model!!!");
                return;
            }
            
            _botAgent = new AlphaZeroAgent(modelAsset,384);
            
            _players = new Dictionary<Player, IAgent>
            {
                { Player.X, _humanAgent },
                { Player.O, _botAgent }
            };
            _currentGameState = GameSetup.SetupNewGame();

            // var selectedMove = await _botAgent.SelectMove(_currentGameState);
            // Debug.Log(selectedMove);

            Debug.Log("Human vs Human Console Test Game started!");
            await StartNextTurn();
        }

        public async void LogInput()
        {
            if (inputField != null)
            {
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
            if (_currentGameState.IsOver())
            {
                Debug.LogWarning("Attempted to apply move to a game that is already over.");
                EndGame();
                return;
            }

            try
            {
                _currentGameState = _currentGameState.ApplyMove(move);
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
            if (gameStateText != null) gameStateText.text = _currentGameState.ToString();
            // Debug.Log(_currentGameState.ToString()); // Console log
            
            Player? winner = _currentGameState.Winner();
            if (winner.HasValue)
            {
                if (gameStateText != null) gameStateText.text += $"\nWinner: {winner.Value}";
            }
            else
            {
                if (gameStateText != null) gameStateText.text += "\nIt's a draw.";
            }
        }
    }
}