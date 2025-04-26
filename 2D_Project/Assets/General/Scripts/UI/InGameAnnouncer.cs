using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
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
        [SerializeField] Button resetButton;
        [SerializeField] Button homeButton;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(OnChangePlayer);
            endGameChannel.AddListener(ShowPanel);
            resetButton.onClick.AddListener(OnClickReset);
            homeButton.onClick.AddListener(OnBackToHome);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(OnChangePlayer);
            endGameChannel.RemoveListener(ShowPanel);
            resetButton.onClick.RemoveListener(OnClickReset);
            homeButton.onClick.AddListener(OnBackToHome);
        }

        private void ShowPanel(bool hasWinner)
        {
            endGamePanel.SetActive(true);
            winPlayerText.text = hasWinner ? $"PLAYER {gameStateStorage.GetValue().Winner()} WIN!" : "DRAW GAME";
        }
        
        private void OnChangePlayer()
        {
            playerText.SetText($"Player {gameStateStorage.GetValue().NextPlayer}");
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