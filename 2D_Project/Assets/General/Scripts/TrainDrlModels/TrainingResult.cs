using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TrainingResult : MonoBehaviour
    {
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private TextMeshProUGUI gameResultText;

        private void OnEnable()
        {
            endGameChannel.AddListener(ShowEpisodeResult);
        }

        private void OnDisable()
        {
            endGameChannel.RemoveListener(ShowEpisodeResult);
        }

        private void ShowEpisodeResult(bool hasWinner)
        {
            if (hasWinner)
            {
                gameResultText.text = $"PLAYER {currentPlayer.GetValue()} WIN!";
                gameResultText.color = Color.green;
            }
            else
            {
                gameResultText.text = $"NO WINNER!";
                gameResultText.color = Color.red;
            }
        }
    }
}
