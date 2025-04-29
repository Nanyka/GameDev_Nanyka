using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class InGameSettings : MonoBehaviour
    {
        [Header("Setting channels")]
        [SerializeField] private LoadEventChannel loadMenu;
        [SerializeField] private GameSceneSO menuToLoad;
        [SerializeField] private SettingsSO currentSettings;
        [SerializeField] private FloatChannel changeSfxVolumeChannel;
        [SerializeField] private SaveSystemManager saveSystem;

        [Header("UI elements")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private GameObject settingContainer;

        private float soundVolume;

        private void OnEnable()
        {
            resumeButton.onClick.AddListener(OnReturnGame);
            homeButton.onClick.AddListener(OnBackToHome);
            soundSlider.onValueChanged.AddListener(OnSoundVolumeChange);
        }

        private void OnDisable()
        {
            resumeButton.onClick.RemoveListener(OnReturnGame);
            homeButton.onClick.RemoveListener(OnBackToHome);
            soundSlider.onValueChanged.RemoveListener(OnSoundVolumeChange);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            soundSlider.value = currentSettings.SfxVolume;
        }

        private void OnReturnGame()
        {
            currentSettings.SaveAudioSettings(soundVolume);
            changeSfxVolumeChannel.ExecuteChannel(soundVolume);
            saveSystem.SaveSettings(currentSettings);
            settingContainer.SetActive(false);
        }

        private void OnBackToHome()
        {
            loadMenu.RaiseEvent(menuToLoad,true,true);
        }

        private void OnSoundVolumeChange(float volume)
        {
            soundVolume = volume;
        }
    }
}