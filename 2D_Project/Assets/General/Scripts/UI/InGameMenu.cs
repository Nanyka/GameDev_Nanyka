using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private VoidChannel enableButtonChannel;
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] Button changePlayerButton;
        [SerializeField] TextMeshProUGUI playerText;

        private void Awake()
        {
            changePlayerButton.onClick.AddListener(OnChangePlayer);
        }

        private void OnEnable()
        {
            enableButtonChannel.AddListener(EnableButton);
            resetGameChannel.AddListener(ResetUI);
        }

        private void OnDisable()
        {
            enableButtonChannel.RemoveListener(EnableButton);
            resetGameChannel.RemoveListener(ResetUI);
        }

        private void EnableButton()
        {
            changePlayerButton.interactable = true;
        }

        private void OnChangePlayer()
        {
            changePlayerChannel.ExecuteChannel();
            changePlayerButton.interactable = false;
            playerText.SetText($"Player {currentPlayer.GetValue()}");
        }
        
        private void ResetUI()
        {
            playerText.SetText($"Player {currentPlayer.GetValue()}");
            changePlayerButton.interactable = false;
        }
    }
}