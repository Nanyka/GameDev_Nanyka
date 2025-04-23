using System;
using AlphaZeroAlgorithm;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class GamePresenter : MonoBehaviour
    {
        [SerializeField] private GameStateStorage gameStateStorage;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private BoolChannel endGameChannel;
        // [SerializeField] private TextMeshProUGUI inGameText;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(ShowGameState);
            endGameChannel.AddListener(ShowEndGame);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(ShowGameState);
            endGameChannel.RemoveListener(ShowEndGame);
        }

        private void ShowGameState()
        {
            Debug.Log(gameStateStorage.GetValue().ToString());
            // inGameText.text = gameStateStorage.GetValue().NextPlayer.ToString();
        }

        private void ShowEndGame(bool isEndGame)
        {
            Player? winner = gameStateStorage.GetValue().Winner();
            // inGameText.text = winner.HasValue ? $"\nWinner: {winner.Value}" : "\nIt's a draw.";
        }
    }
}