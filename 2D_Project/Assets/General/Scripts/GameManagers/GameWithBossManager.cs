using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaZeroAlgorithm;
using UnityEngine;

namespace TheAiAlchemist
{
    public class GameWithBossManager : GameManager
    {
        protected override async Task<bool> LoadDecisionMakers()
        {
            var loadResult = true;
            _humanAgent = new AlphaZeroAlgorithm.Human();

            // Debug.Log($"Loading model {modelAddress[saveSystemManager.saveData.level]}");
            var modelAsset = await addressableManager.GetModel(modelAddress[^1]);
            if (modelAsset == null)
            {
                Debug.Log("The game fail to load model!!!");
                loadResult = false;
            }

            _botAgent?.DisableAiElements();
            var roundPerMove = 384;
            _botAgent = new AlphaZeroAgent(modelAsset, roundPerMove);

            _players = new Dictionary<Player, IAgent>
            {
                { Player.O, _humanAgent },
                { Player.X, _botAgent }
            };

            return loadResult;
        }
        
        protected override async Task ApplySelectedMove(Move move)
        {
            if (_currentGameState.IsOver())
            {
                Debug.LogWarning("Attempted to apply move to a game that is already over.");
                EndGame();
                return;
            }

            try
            {
                var point = move.Point;
                var player = _currentGameState.Board.GetPlayerAtCoord(point.Row, point.Col);
                _currentGameState = _currentGameState.ApplyMove(move);
                audioPlayIndex.ExecuteChannel(player.HasValue ? 1 : 0);

                await StartNextTurn();
            }
            catch (IllegalMoveError ex)
            {
                Debug.LogError($"Illegal move applied by {_currentGameState.NextPlayer}: {ex.Message}. " +
                               $"Game state remains unchanged.");
                if (_currentGameState.NextPlayer != Player.O)
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
        
        protected override void EndTurnSetup()
        {
            gameStateStorage.SetValue(_currentGameState);
            changePlayerChannel.ExecuteChannel();
            botThinkingChannel.ExecuteChannel(_currentGameState.NextPlayer == Player.X);
            askUnitIndex.SetValue(-1);
        }
        
        protected override async void EndGame()
        {
            if (_isEndGame)
                return;
            _isEndGame = true;

            gameStateStorage.SetValue(_currentGameState);
            changePlayerChannel.ExecuteChannel();
            endGameChannel.ExecuteChannel(_currentGameState.Winner() != null);
        }
    }
}