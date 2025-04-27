using System;
using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private VoidChannel onClickNewGameButton;
        [SerializeField] private VoidChannel onClickTutorialButton;
        [SerializeField] private VoidChannel onClickResetButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button resetButton;

        private void OnEnable()
        {
            newGameButton.onClick.AddListener(OnClickNewGame);
            tutorialButton.onClick.AddListener(OnClickTutorial);
            resetButton.onClick.AddListener(OnClickReset);
        }
        
        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(OnClickNewGame);
            tutorialButton.onClick.AddListener(OnClickTutorial);
            resetButton.onClick.AddListener(OnClickReset);
        }

        private void OnClickNewGame()
        {
            onClickNewGameButton.ExecuteChannel();
        }

        private void OnClickTutorial()
        {
            onClickTutorialButton.ExecuteChannel();
        }
        
        private void OnClickReset()
        {
            onClickResetButton.ExecuteChannel();
        }
    }
}