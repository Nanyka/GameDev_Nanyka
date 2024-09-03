using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TheAiAlchemist
{
    public class GameStateForTrainingGpTwo : MonoBehaviour
    {
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private VoidChannel newGameChannel;
        [SerializeField] private VoidChannel interruptGameChannel;
        [SerializeField] private CircleChannel announceStateChanged;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private ListCircleStorage gameBoard;
        [SerializeField] private Vector3Storage gameBoardPos;
        [SerializeField] private IndexAndPlotTranslator winRuler;
        [SerializeField] private List<IPlayerBehaviorStorage> players;

        [SerializeField] private int currentPlayerIndex;

        // private int circleAmount;
        private bool isNewStep;

        private void OnEnable()
        {
            resetGameChannel.AddListener(ResetCurrentPlayer);
            announceStateChanged.AddListener(StateChanged);
            interruptGameChannel.AddListener(GameInterrupted);
        }

        private void OnDisable()
        {
            resetGameChannel.RemoveListener(ResetCurrentPlayer);
            announceStateChanged.RemoveListener(StateChanged);
            interruptGameChannel.RemoveListener(GameInterrupted);
        }

        private void Start()
        {
            gameBoardPos.SetValue(transform.position);
            ResetCurrentPlayer();
        }

        private void FixedUpdate()
        {
            if (isNewStep)
            {
                isNewStep = false;
                MoveToNextStep();
            }
        }

        private void SetCurrentPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            currentPlayer.SetValue(players[currentPlayerIndex].GetValue().GetPlayerId());
        }

        private void StateChanged(ICircleTrait circleTrait)
        {
            // Record environment state
            gameBoard.GetValue()[circleTrait.GetId()] = circleTrait;
            isNewStep = true;
        }

        private void MoveToNextStep()
        {
            var isWin = CheckWinCondition();
            if (isWin)
            {
                endGameChannel.ExecuteChannel(true);
                resetGameChannel.ExecuteChannel();
            }
            else
                IncreaseCircleAmount();
        }

        private bool CheckWinCondition()
        {
            bool isWin = false;
            var positionList = new List<int>();
            for (int i = 0; i < gameBoard.GetValue().Count; i++)
            {
                if (gameBoard.GetValue()[i] == null)
                    continue;

                if (currentPlayer.GetValue() == gameBoard.GetValue()[i].GetPlayerId())
                    positionList.Add(i);
            }

            foreach (var combination in winRuler.winningCombinations)
            {
                if (positionList.Contains(combination.Item1)
                    && positionList.Contains(combination.Item2)
                    && positionList.Contains(combination.Item3))
                {
                    isWin = true;
                    break;
                }
            }

            return isWin;
        }

        private void IncreaseCircleAmount()
        {
            // circleAmount++;
            if (CheckDrawState())
            {
                endGameChannel.ExecuteChannel(false);
                resetGameChannel.ExecuteChannel();
            }
            else
            {
                SetCurrentPlayer();
                changePlayerChannel.ExecuteChannel();
            }
        }

        private bool CheckDrawState()
        {
            // Check is it any available slot --> if true, return true
            // Check is any opponent slot has the minimum priority lower than the maximum priority of the player
            // --> If true, return true
            // Return false

            // var circles = gameBoard.GetValue();
            // for (int i = 0; i < circles.Count; i++)
            // {
            //     if (circles[i] == null)
            //         return true;
            //
            // }

            // Compute total priority of the current player. If it is larger than 10, then the game draw
            var currentPlayerCircles = gameBoard.GetValue().FindAll(t => t != null);
            currentPlayerCircles = currentPlayerCircles.FindAll(t => t.GetPlayerId() == currentPlayer.GetValue());
            // Debug.Log($"Player {currentPlayer.GetValue()} priority: {currentPlayerCircles.Sum(t => t.GetPriority())}");
            var totalPriority = currentPlayerCircles.Sum(t => t.GetPriority());
            var isEmptyInventory = players[currentPlayerIndex].GetValue().GetInventory().IsEmpty();
            return (totalPriority >= 9 && gameBoard.GetValue().Count(t => t == null) == 0) ||
                   totalPriority >= 11 || isEmptyInventory;
        }

        private void ResetCurrentPlayer()
        {
            // circleAmount = 0;
            gameBoard.ResetList();
            currentPlayer.SetValue(players[currentPlayerIndex].GetValue().GetPlayerId());

            // Ask current player to play
            newGameChannel.ExecuteChannel();
        }

        private void GameInterrupted()
        {
            endGameChannel.ExecuteChannel(false);
            resetGameChannel.ExecuteChannel();
        }
    }
}