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
        [SerializeField] private CombatChannel combatChannel;
        [SerializeField] private TextMeshProUGUI gameResultText;

        private void OnEnable()
        {
            endGameChannel.AddListener(ShowEpisodeResult);
            combatChannel.AddListener(ShowCombat);
        }

        private void OnDisable()
        {
            endGameChannel.RemoveListener(ShowEpisodeResult);
            combatChannel.RemoveListener(ShowCombat);
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
        
        private void ShowCombat(int attacker, bool isAttack)
        {
            gameResultText.text = "With COMBAT";
            gameResultText.color = Color.yellow;
        }
    }
}
