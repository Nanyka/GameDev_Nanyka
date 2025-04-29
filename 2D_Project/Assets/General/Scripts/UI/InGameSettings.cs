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
        [SerializeField] private VoidChannel changeSettingsChannel;
        [SerializeField] private SaveSystemManager saveSystem;

        [Header("UI elements")]
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button homeButton;
        [SerializeField] private Slider soundSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private GameObject settingContainer;

        private float soundVolume;
        private float musicVolume;

        private void OnEnable()
        {
            resumeButton.onClick.AddListener(OnReturnGame);
            homeButton.onClick.AddListener(OnBackToHome);
            soundSlider.onValueChanged.AddListener(OnSoundVolumeChange);
            musicSlider.onValueChanged.AddListener(OnMusicVolumeChange);
        }

        private void OnDisable()
        {
            resumeButton.onClick.RemoveListener(OnReturnGame);
            homeButton.onClick.RemoveListener(OnBackToHome);
            soundSlider.onValueChanged.RemoveListener(OnSoundVolumeChange);
            musicSlider.onValueChanged.RemoveListener(OnMusicVolumeChange);
        }

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            soundVolume = currentSettings.SfxVolume;
            musicVolume = currentSettings.MusicVolume;
            soundSlider.value = soundVolume;
            musicSlider.value = musicVolume;
        }

        private void OnReturnGame()
        {
            currentSettings.SaveAudioSettings(soundVolume, musicVolume);
            saveSystem.SaveSettings(currentSettings);
            settingContainer.SetActive(false);
            changeSettingsChannel.ExecuteChannel();
        }

        private void OnBackToHome()
        {
            loadMenu.RaiseEvent(menuToLoad,true,true);
        }

        private void OnSoundVolumeChange(float volume)
        {
            soundVolume = volume;
            currentSettings.SaveAudioSettings(soundVolume, musicVolume);
            changeSettingsChannel.ExecuteChannel();
        }

        private void OnMusicVolumeChange(float volume)
        {
            musicVolume = volume;
            currentSettings.SaveAudioSettings(soundVolume, musicVolume);
            changeSettingsChannel.ExecuteChannel();
        }
    }
}