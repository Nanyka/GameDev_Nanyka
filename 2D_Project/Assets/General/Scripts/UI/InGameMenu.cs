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
        [SerializeField] Button _changePlayerButton;
        [SerializeField] TextMeshProUGUI _playerText;

        private void Awake()
        {
            _changePlayerButton.onClick.AddListener(OnChangePlayer);
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
            _changePlayerButton.interactable = true;
        }

        private void OnChangePlayer()
        {
            currentPlayer.SetValue((currentPlayer.GetValue() + 1) % 2);
            _playerText.SetText(currentPlayer.GetValue() == 0 ? "Player1" : "Player2");

            changePlayerChannel.ExecuteChannel();
            _changePlayerButton.interactable = false;
        }
        
        private void ResetUI()
        {
            _playerText.SetText(currentPlayer.GetValue() == 0 ? "Player1" : "Player2");
            _changePlayerButton.interactable = false;
        }
    }
}