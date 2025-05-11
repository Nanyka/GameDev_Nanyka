using System.Collections;
using AlphaZeroAlgorithm;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class InGameAnnouncer : MonoBehaviour
    {
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] protected GameStateStorage gameStateStorage;
        [SerializeField] private LoadEventChannel loadMenu;
        [SerializeField] private GameSceneSO menuToLoad;

        [SerializeField] TextMeshProUGUI playerText;
        [SerializeField] protected GameObject endGamePanel;
        [SerializeField] protected TextMeshProUGUI winPlayerText;
        [SerializeField] protected Button playAgainButton;
        [SerializeField] Button homeButton;

        [SerializeField] protected float waitToShowWin = 0f;
        [SerializeField] protected Player playerFaction;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(OnChangePlayer);
            endGameChannel.AddListener(ShowPanel);
            playAgainButton.onClick.AddListener(OnClickReset);
            homeButton.onClick.AddListener(OnBackToHome);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(OnChangePlayer);
            endGameChannel.RemoveListener(ShowPanel);
            playAgainButton.onClick.RemoveListener(OnClickReset);
            homeButton.onClick.AddListener(OnBackToHome);
        }

        private void ShowPanel(bool hasWinner)
        {
            StartCoroutine(WaitToShowWin(hasWinner));
        }

        protected virtual IEnumerator WaitToShowWin(bool hasWinner)
        {
            yield return new WaitForSeconds(waitToShowWin);
            
            endGamePanel.SetActive(true);
            
            string winningText = "";
            if (hasWinner)
                winningText = gameStateStorage.GetValue().Winner() == playerFaction ? "Victory in yours !!!" : 
                    "The bot triumphs this time !";
            else winningText = "It is stalemate !";
            
            winPlayerText.text = winningText;
            
            var buttonText = playAgainButton.GetComponentInChildren<TextMeshProUGUI>();
            
            if (hasWinner && gameStateStorage.GetValue().Winner() == Player.X)
            {
                buttonText.text = "Next Challenge";
            }
            else
                buttonText.text = "Play again";
        }
        
        private void OnChangePlayer()
        {
            var whoTurn = gameStateStorage.GetValue().NextPlayer == playerFaction ? "Your turn" : "Bot turn";
            playerText.SetText(whoTurn);
        }
        
        private void OnClickReset()
        {
            endGamePanel.SetActive(false);
            resetGameChannel.ExecuteChannel();
        }

        private void OnBackToHome()
        {
            loadMenu.RaiseEvent(menuToLoad,true,true);
        }
    }
}