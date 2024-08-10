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
        [SerializeField] private BoolChannel endGameChannel;
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

        private void ShowPanel(bool hasWinner)
        {
            endGamePanel.SetActive(true);
            winPlayerText.text = hasWinner ? $"PLAYER {currentPlayer.GetValue()} WIN!" : "DRAW GAME";
        }

        private void OnClickReset()
        {
            endGamePanel.SetActive(false);
            resetGameChannel.ExecuteChannel();
        }
    }
}
