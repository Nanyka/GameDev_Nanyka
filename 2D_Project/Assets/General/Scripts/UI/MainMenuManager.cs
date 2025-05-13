using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private VoidChannel onClickNewGameButton;
        [SerializeField] private VoidChannel onClickTutorialButton;
        [SerializeField] private VoidChannel onClickResetButton;
        [SerializeField] private VoidChannel onClickBossGameButton;
        [SerializeField] private IntChannel sfxPlayIndex;
        [SerializeField] private SaveSystemManager saveSystem;

        [SerializeField] private Button newGameButton;
        [SerializeField] private Button tutorialButton;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button bossGameButton;
        [SerializeField] private GameObject bossUnlockInfoRegion;

        [SerializeField] private int unlockBosLevel;

        private void OnEnable()
        {
            newGameButton.onClick.AddListener(OnClickNewGame);
            tutorialButton.onClick.AddListener(OnClickTutorial);
            resetButton.onClick.AddListener(OnClickReset);
            bossGameButton.onClick.AddListener(OnClickBossGame);
        }
        
        private void OnDisable()
        {
            newGameButton.onClick.RemoveListener(OnClickNewGame);
            tutorialButton.onClick.RemoveListener(OnClickTutorial);
            resetButton.onClick.RemoveListener(OnClickReset);
            bossGameButton.onClick.RemoveListener(OnClickBossGame);
        }

        private void Start()
        {
            if (saveSystem.saveData.level < unlockBosLevel)
            {
                bossGameButton.interactable = false;
                bossUnlockInfoRegion.SetActive(true);
            }
            else
            {
                bossGameButton.interactable = true;
                bossUnlockInfoRegion.SetActive(false);
            }
        }

        private void OnClickNewGame()
        {
            onClickNewGameButton.ExecuteChannel();
            sfxPlayIndex.ExecuteChannel(2);
        }

        private void OnClickTutorial()
        {
            onClickTutorialButton.ExecuteChannel();
            sfxPlayIndex.ExecuteChannel(2);
        }
        
        private void OnClickReset()
        {
            onClickResetButton.ExecuteChannel();
            sfxPlayIndex.ExecuteChannel(2);
        }
        
        private void OnClickBossGame()
        {
            onClickBossGameButton.ExecuteChannel();
            sfxPlayIndex.ExecuteChannel(2);
        }
    }
}