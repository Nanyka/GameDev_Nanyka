using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaZeroAlgorithm;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private GameStateStorage gameStateStorage;
        [SerializeField] private IntStorage askUnitIndex;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private MoveChannel humanMoveChannel;
        [SerializeField] private VoidChannel resetChannel;
        // [SerializeField] private IntChannel audioPlayIndex;
        [SerializeField] protected VoidChannel finishDropChannel;

        private IAgent _humanAgent;
        private TutorialBot _botAgent;
        private GameState _currentGameState;
        private Dictionary<Player, IAgent> _players;

        private void OnEnable()
        {
            humanMoveChannel.AddListener(HumanPlayAMove);
            resetChannel.AddListener(ResetGame);
            finishDropChannel.AddListener(GetNextPlayerMove);
        }

        private void OnDisable()
        {
            humanMoveChannel.RemoveListener(HumanPlayAMove);
            resetChannel.RemoveListener(ResetGame);
            finishDropChannel.RemoveListener(GetNextPlayerMove);
        }

        private async void Start()
        {
            await Init();
        }

        private Task Init()
        {
            _humanAgent = new AlphaZeroAlgorithm.Human();
            _botAgent = new TutorialBot();
            _botAgent.Init();
            
            _players = new Dictionary<Player, IAgent>
            {
                { Player.X, _humanAgent },
                { Player.O, _botAgent }
            };
            
            resetChannel.ExecuteChannel();
            return Task.CompletedTask;
        }

        private async void HumanPlayAMove(Move humanMove)
        {
            await ApplySelectedMove(humanMove);
        }

        private async Task StartNextTurn()
        {
            if (_currentGameState.IsOver())
            {
                EndGame();
                return;
            }
            
            gameStateStorage.SetValue(_currentGameState); 
            changePlayerChannel.ExecuteChannel();
            askUnitIndex.SetValue(-1);

            // var nextMove = await _players[_currentGameState.NextPlayer].SelectMove(_currentGameState);
            // if (nextMove != null) await ApplySelectedMove(nextMove);
        }
        
        private async void GetNextPlayerMove()
        {
            if (_currentGameState.IsOver())
                return;
            var nextMove = await _players[_currentGameState.NextPlayer].SelectMove(_currentGameState);
            if (nextMove != null) await ApplySelectedMove(nextMove);
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
                var isOnGrid = Board.IsOnGrid(move.Point);
                _currentGameState = _currentGameState.ApplyMove(move);
                // audioPlayIndex.ExecuteChannel(isOnGrid ? 1 : 0);
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
            gameStateStorage.SetValue(_currentGameState);
            changePlayerChannel.ExecuteChannel();
            endGameChannel.ExecuteChannel(_currentGameState.Winner()!=null);
        }

        private async void ResetGame()
        {
            _currentGameState = GameSetup.SetupNewGame();
            await StartNextTurn();
            GetNextPlayerMove();
        }
    }
}