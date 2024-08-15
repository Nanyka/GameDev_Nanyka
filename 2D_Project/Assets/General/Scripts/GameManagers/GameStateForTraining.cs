using System;
using System.Collections.Generic;
using UnityEngine;

namespace TheAiAlchemist
{
    public class GameStateForTraining : MonoBehaviour
    {
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private VoidChannel newGameChannel;
        [SerializeField] private VoidChannel interruptGameChannel;
        [SerializeField] private TwoIntChannel announceStateChanged;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private ListIntStorage gameBoard;
        [SerializeField] private Vector3Storage gameBoardPos;
        [SerializeField] private IndexAndPlotTranslator winRuler;
        [SerializeField] private List<IPlayerBehaviorStorage> players;

        [SerializeField] private int currentPlayerIndex;
        private int circleAmount;
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

        private void StateChanged(int playerId, int location)
        {
            // Record environment state
            gameBoard.GetValue()[location] = playerId;
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
                if (currentPlayer.GetValue() == gameBoard.GetValue()[i])
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
            circleAmount++;
            if (circleAmount >= 9)
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

        private void ResetCurrentPlayer()
        {
            circleAmount = 0;
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