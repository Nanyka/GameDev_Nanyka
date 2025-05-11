using System.Collections;
using TMPro;
using UnityEngine;
using AlphaZeroAlgorithm;

namespace TheAiAlchemist
{
    public class BossInGameAnnouncer : InGameAnnouncer
    {
        protected override IEnumerator WaitToShowWin(bool hasWinner)
        {
            yield return new WaitForSeconds(waitToShowWin);
            
            endGamePanel.SetActive(true);

            string winningText = "";
            if (hasWinner)
                winningText = gameStateStorage.GetValue().Winner() == playerFaction ? "Victory in yours !!!" : 
                    "The bot triumphs this time !";
            else winningText = "It is stalemate !";
            
            winPlayerText.text = winningText;
            // var buttonText = playAgainButton.GetComponentInChildren<TextMeshProUGUI>();
            
            if (hasWinner && gameStateStorage.GetValue().Winner() == playerFaction)
                playAgainButton.gameObject.SetActive(false);
        }
    }
}