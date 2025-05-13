using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaZeroAlgorithm;
using TMPro;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] protected GameStateStorage gameStateStorage;
        [SerializeField] protected IntStorage askUnitIndex;
        [SerializeField] protected VoidChannel changePlayerChannel;
        [SerializeField] protected BoolChannel endGameChannel;
        [SerializeField] private MoveChannel humanMoveChannel;
        [SerializeField] private VoidChannel resetChannel;
        [SerializeField] protected IntChannel audioPlayIndex;
        [SerializeField] protected BoolChannel botThinkingChannel;
        [SerializeField] private VoidChannel checkIapState;
        [SerializeField] private BoolChannel iapStateChannel;
        [SerializeField] private RemoteConfigManagerSO remoteConfigManager;
        [SerializeField] private VoidChannel readyForGame;
        
        [SerializeField] protected AddressableManagerSO addressableManager;
        [SerializeField] protected SaveSystemManager saveSystemManager;
        [SerializeField] protected string[] modelAddress;

        protected IAgent _humanAgent;
        protected AlphaZeroAgent _botAgent;
        protected GameState _currentGameState;
        protected Dictionary<Player, IAgent> _players;
        protected bool _isEndGame;

        private void OnEnable()
        {
            humanMoveChannel.AddListener(HumanPlayAMove);
            resetChannel.AddListener(ResetGame);
            iapStateChannel.AddListener(StartGame);
            readyForGame.AddListener(ReadyForGame);
        }

        private void OnDisable()
        {
            humanMoveChannel.RemoveListener(HumanPlayAMove);
            resetChannel.RemoveListener(ResetGame);
            iapStateChannel.RemoveListener(StartGame);
            readyForGame.RemoveListener(ReadyForGame);

            _botAgent?.DisableAiElements();
        }

        private async void ReadyForGame()
        {
            await Init();
        }

        private async void Start()
        {
            // await Init();
        }

        private async Task Init()
        {
            var loadPlayers = await LoadDecisionMakers();
            if (loadPlayers == false)
                return;

            resetChannel.ExecuteChannel();
        }

        protected virtual async Task<bool> LoadDecisionMakers()
        {
            var loadResult = true;
            _humanAgent = new AlphaZeroAlgorithm.Human();

            // Debug.Log($"Loading model {modelAddress[saveSystemManager.saveData.level]}");
            var modelAsset = await addressableManager.GetModel(modelAddress[saveSystemManager.saveData.level]);
            if (modelAsset == null)
            {
                Debug.Log("The game fail to load model!!!");
                loadResult = false;
            }

            _botAgent?.DisableAiElements();
            var roundPerMove = 128*Mathf.Min(3, saveSystemManager.saveData.level + 1);
            _botAgent = new AlphaZeroAgent(modelAsset, roundPerMove);

            _players = new Dictionary<Player, IAgent>
            {
                { Player.X, _humanAgent },
                { Player.O, _botAgent }
            };

            return loadResult;
        }

        private async void HumanPlayAMove(Move humanMove)
        {
            await ApplySelectedMove(humanMove);
        }

        protected async Task StartNextTurn()
        {
            if (_currentGameState.IsOver())
            {
                EndGame();
                return;
            }

            EndTurnSetup();

            var nextMove = await _players[_currentGameState.NextPlayer].SelectMove(_currentGameState);
            if (nextMove != null) await ApplySelectedMove(nextMove);
        }

        protected virtual async Task ApplySelectedMove(Move move)
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
        
        protected virtual void EndTurnSetup()
        {
            gameStateStorage.SetValue(_currentGameState);
            changePlayerChannel.ExecuteChannel();
            botThinkingChannel.ExecuteChannel(_currentGameState.NextPlayer == Player.O);
            askUnitIndex.SetValue(-1);
        }

        protected virtual async void EndGame()
        {
            if (_isEndGame)
                return;
            _isEndGame = true;

            gameStateStorage.SetValue(_currentGameState);
            changePlayerChannel.ExecuteChannel();

            // Level up if human (play X) win
            if (_currentGameState.Winner() != null && _currentGameState.Winner() == Player.X)
            {
                saveSystemManager.saveData.level +=
                    Mathf.Min(1, modelAddress.Length - saveSystemManager.saveData.level - 1);
                saveSystemManager.SaveDataToDisk();

                var loadPlayers = await LoadDecisionMakers();
                if (loadPlayers == false)
                    return;
            }

            endGameChannel.ExecuteChannel(_currentGameState.Winner() != null);
        }

        private void ResetGame()
        {
            if (saveSystemManager.saveData.level <= remoteConfigManager.GetNumericConfig(NumericConfigName.COFFEE_ASK)) 
                StartGame(true);
            else
                checkIapState.ExecuteChannel();
        }
        
        private async void StartGame(bool iapState)
        {
            _isEndGame = false;
            _currentGameState = GameSetup.SetupNewGame();
            await StartNextTurn();
            
            // if (iapState)
            // {
            //     _isEndGame = false;
            //     _currentGameState = GameSetup.SetupNewGame();
            //     await StartNextTurn();
            // }
            // else checkIapState.ExecuteChannel();
        }
    }
}