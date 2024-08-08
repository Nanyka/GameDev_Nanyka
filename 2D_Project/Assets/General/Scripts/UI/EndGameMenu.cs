using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class EndGameMenu : MonoBehaviour
    {
        [SerializeField] private VoidChannel endGameChannel;
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private Button resetButton;
        [SerializeField] private TextMeshProUGUI winPlayerText;

        private void OnEnable()
        {
            endGameChannel.AddListener(ShowPanel);
            resetButton.onClick.AddListener(OnClickReset);
        }
        
        private void OnDisable()
        {
            endGameChannel.RemoveListener(ShowPanel);
        }

        private void ShowPanel()
        {
            endGamePanel.SetActive(true);
            winPlayerText.text = $"PLAYER {currentPlayer.GetValue()} WIN!";
        }

        private void OnClickReset()
        {
            endGamePanel.SetActive(false);
            resetGameChannel.ExecuteChannel();
        }
    }
}
