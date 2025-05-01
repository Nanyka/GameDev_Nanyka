using TheAiAlchemist;
using UnityEngine;
using UnityEngine.Serialization;

// using UnityEngine.Localization;
// using UnityEngine.Localization.Settings;
// using UnityEngine.Rendering.Universal;
namespace TheAiAlchemist
{
    public class SettingsSystem : MonoBehaviour
    {
        [SerializeField] private VoidChannel SaveSettingsEvent = default;
        [SerializeField] private SettingsSO _currentSettings = default;
        [SerializeField] private SaveSystemManager _saveSystem = default;
        [SerializeField] private VoidChannel changeSettingsChannel;
        [SerializeField] private SettingsSO settingsSo = default;

        private void Awake()
        {
            var hasSaveData = _saveSystem.LoadSaveDataFromDisk();
            if (hasSaveData == false)
            {
                _saveSystem.WriteEmptySaveFile();
                _saveSystem.saveData.musicVolume = 0.5f;
                _saveSystem.saveData.sfxVolume = 0.5f;
                //TODO load from cloud and check playerId here
                _saveSystem.SetNewGameData();
            }
            
            _currentSettings.LoadSavedSettings(_saveSystem.saveData);
            SetCurrentSettings();
        }

        private void OnEnable()
        {
            SaveSettingsEvent.AddListener(SaveSettings);
        }

        private void OnDisable()
        {
            SaveSettingsEvent.RemoveListener(SaveSettings);
        }

        void SetCurrentSettings()
        {
            changeSettingsChannel.ExecuteChannel(); 
        }

        void SaveSettings()
        {
            _saveSystem.SaveDataToDisk();
        }
    }
}