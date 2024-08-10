using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Apply dedicated settings following specified state of the game
// Reset game: reset currentPlayer to playerId = 0

namespace TheAiAlchemist
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private TwoIntChannel announceStateChanged;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private List<IPlayerBehaviorStorage> players;

        private int currentPlayerIndex;
        private int circleAmount;
        private List<int> gameBoard = new(new int[9]);
        private (int, int, int)[] winningCombinations = {
            (0, 1, 2), (3, 4, 5), (6, 7, 8), // Horizontal
            (0, 3, 6), (1, 4, 7), (2, 5, 8), // Vertical
            (0, 4, 8), (2, 4, 6) // Diagonal
        };

        private void OnEnable()
        {
            resetGameChannel.AddListener(ResetCurrentPlayer);
            announceStateChanged.AddListener(StateChanged);
            changePlayerChannel.AddListener(SetCurrentPlayer);
        }
        
        private void OnDisable()
        {
            resetGameChannel.RemoveListener(ResetCurrentPlayer);
            announceStateChanged.RemoveListener(StateChanged);
            changePlayerChannel.RemoveListener(SetCurrentPlayer);
        }

        // TODO: consider to execute it with a START GAME button
        private void Start()
        {
            ResetCurrentPlayer();
        }

        private void SetCurrentPlayer()
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
            currentPlayer.SetValue(players[currentPlayerIndex].GetValue().GetPlayerId());
        }

        private void StateChanged(int playerId, int location)
        {
            // Record environment state
            gameBoard[location] = playerId;
            
            var isWin = CheckWinCondition();
            if (isWin)
                endGameChannel.ExecuteChannel(true);
            else
                IncreaseCircleAmount();
        }

        private bool CheckWinCondition()
        {
            bool isWin = false;
            var positionList = new List<int>();
            for (int i = 0; i < gameBoard.Count; i++)
            {
                if (currentPlayer.GetValue() == gameBoard[i])
                    positionList.Add(i);
            }

            foreach (var combination in winningCombinations)
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
                endGameChannel.ExecuteChannel(false);
        }

        private void ResetCurrentPlayer()
        {
            circleAmount = 0;
            currentPlayerIndex = 0;
            currentPlayer.SetValue(players[currentPlayerIndex].GetValue().GetPlayerId());
            for (int i = 0; i < gameBoard.Count(); i++)
                gameBoard[i] = 0;
        }
    }
}