using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private VoidChannel onClickNewGameButton;
        [SerializeField] private Button newGameButton;

        private void OnEnable()
        {
            newGameButton.onClick.AddListener(OnClickNewGame);
        }
        
        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(OnClickNewGame);
        }

        private void OnClickNewGame()
        {
            onClickNewGameButton.ExecuteChannel();
        }
    }
}