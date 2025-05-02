using System.Collections;
using AlphaZeroAlgorithm;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    using UnityEngine;
    using AlphaZeroAlgorithm;


    public class InGameAnnouncer : MonoBehaviour
    {
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private GameStateStorage gameStateStorage;
        [SerializeField] private LoadEventChannel loadMenu;
        [SerializeField] private GameSceneSO menuToLoad;

        [SerializeField] TextMeshProUGUI playerText;
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private TextMeshProUGUI winPlayerText;
        [SerializeField] Button playAgainButton;
        [SerializeField] Button homeButton;

        [SerializeField] private float waitToShowWin = 0f;

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
            // endGamePanel.SetActive(true);
            // winPlayerText.text = hasWinner ? $"PLAYER {gameStateStorage.GetValue().Winner()} WIN!" : "DRAW GAME";
            // var buttonText = playAgainButton.GetComponentInChildren<TextMeshProUGUI>();
            //
            // if (hasWinner && gameStateStorage.GetValue().Winner() == Player.X)
            // {
            //     buttonText.text = "Next Challenge";
            // }
            // else
            //     buttonText.text = "Play again";
            
            StartCoroutine(WaitToShowWin(hasWinner));
        }

        private IEnumerator WaitToShowWin(bool hasWinner)
        {
            yield return new WaitForSeconds(waitToShowWin);
            
            endGamePanel.SetActive(true);
            winPlayerText.text = hasWinner ? $"PLAYER {gameStateStorage.GetValue().Winner()} WIN!" : "DRAW GAME";
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
            playerText.SetText($"In turn of <b>Player {gameStateStorage.GetValue().NextPlayer}</b>");
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