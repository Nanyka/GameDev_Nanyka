using System;
using UnityEngine;

// Apply dedicated settings following specified state of the game
// Reset game: reset currentPlayer to playerId = 0

namespace TheAiAlchemist
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private IntStorage currentPlayer;

        private void OnEnable()
        {
            resetGameChannel.AddListener(ResetCurrentPlayer);
        }
        
        private void OnDisable()
        {
            resetGameChannel.RemoveListener(ResetCurrentPlayer);
        }

        private void ResetCurrentPlayer()
        {
            currentPlayer.SetValue(0);
        }
    }
}