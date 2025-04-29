using TheAiAlchemist;
using UnityEngine;

// using UnityEngine.Localization;
// using UnityEngine.Localization.Settings;
// using UnityEngine.Rendering.Universal;
namespace TheAiAlchemist
{
    public class SettingsSystem : MonoBehaviour
    {
        [SerializeField] private VoidChannel SaveSettingsEvent = default;

        [SerializeField] private SettingsSO _currentSettings = default;

        // [SerializeField] private UniversalRenderPipelineAsset _urpAsset = default;
        [SerializeField] private SaveSystemManager _saveSystem = default;

        //
        // [SerializeField] private FloatEventChannelSO _changeMasterVolumeEventChannel = default;
        [SerializeField] private FloatChannel _changeSFXVolumeEventChannel;
        // [SerializeField] private FloatEventChannelSO _changeMusicVolumeEventChannel = default;

        private void Awake()
        {
            var hasSaveData = _saveSystem.LoadSaveDataFromDisk();
            if (hasSaveData == false)
            {
                _saveSystem.WriteEmptySaveFile();
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

        /// <summary>
        /// Set current settings 
        /// </summary>
        void SetCurrentSettings()
        {
            // _changeMusicVolumeEventChannel.RaiseEvent(_currentSettings.MusicVolume);//raise event for volume change
            _changeSFXVolumeEventChannel.ExecuteChannel(_currentSettings.SfxVolume); //raise event for volume change
            // _changeMasterVolumeEventChannel.RaiseEvent(_currentSettings.MasterVolume); //raise event for volume change
            // Resolution currentResolution = Screen.currentResolution; // get a default resolution in case saved resolution doesn't exist in the resolution List
            // if (_currentSettings.ResolutionsIndex < Screen.resolutions.Length)
            // 	currentResolution = Screen.resolutions[_currentSettings.ResolutionsIndex];
            // Screen.SetResolution(currentResolution.width, currentResolution.height, _currentSettings.IsFullscreen);
            // _urpAsset.shadowDistance = _currentSettings.ShadowDistance;
            // _urpAsset.msaaSampleCount = _currentSettings.AntiAliasingIndex;
            //
            // LocalizationSettings.SelectedLocale = _currentSettings.CurrentLocale;
        }

        void SaveSettings()
        {
            _saveSystem.SaveDataToDisk();
        }
    }
}