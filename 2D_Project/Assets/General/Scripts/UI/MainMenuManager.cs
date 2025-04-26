using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private VoidChannel onClickNewGameButton;
        [SerializeField] private VoidChannel onClickTutorialButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button tutorialButton;

        private void OnEnable()
        {
            newGameButton.onClick.AddListener(OnClickNewGame);
            tutorialButton.onClick.AddListener(OnClickTutorial);
        }
        
        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(OnClickNewGame);
            tutorialButton.onClick.AddListener(OnClickTutorial);
        }

        private void OnClickNewGame()
        {
            onClickNewGameButton.ExecuteChannel();
        }

        private void OnClickTutorial()
        {
            onClickTutorialButton.ExecuteChannel();
        }
    }
}