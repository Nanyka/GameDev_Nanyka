using System.Threading.Tasks;
using TheAiAlchemist;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Serialization;

// using UnityEngine.Localization;
// using UnityEngine.Localization.Settings;
// using UnityEngine.Rendering.Universal;
namespace TheAiAlchemist
{
    public class SettingsSystem : MonoBehaviour
    {
        [SerializeField] private VoidChannel SaveSettingsEvent;
        [SerializeField] private SettingsSO _currentSettings;
        [SerializeField] private SaveSystemManager _saveSystem;
        [SerializeField] private VoidChannel changeSettingsChannel;
        [SerializeField] private SettingsSO settingsSo;

        private async void Awake()
        {
            var hasSaveData = _saveSystem.LoadSaveDataFromDisk();
            if (hasSaveData == false)
            {
                _saveSystem.WriteEmptySaveFile();
                _saveSystem.saveData.musicVolume = 0.5f;
                _saveSystem.saveData.sfxVolume = 0.5f;
                _saveSystem.SetNewGameData();
            }
            
            _currentSettings.LoadSavedSettings(_saveSystem.saveData);
            await ComparePlayerId();
            // _saveSystem.SavePlayerId("Save a fake Id");
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
        
        private async Task ComparePlayerId()
        {
            var checkId = _saveSystem.saveData.playerId;
            
            // Check internet availability
            if (Application.internetReachability == NetworkReachability.NotReachable) return;
            
            if (checkId == "")
            {
                if (AuthenticationService.Instance.SessionTokenExists)
                {
                    if (!AuthenticationService.Instance.IsSignedIn)
                        await AuthenticationService.Instance.SignInAnonymouslyAsync();

                    // Debug.Log($"Player id when token exist:{AuthenticationService.Instance.PlayerId}");
                    _saveSystem.SavePlayerId(AuthenticationService.Instance.PlayerId);
                }
                
                return;
            }
            
            // Debug.Log($"Player id to check: {checkId}");
            
            // If available, check playerId. If playerId is matched, return true
            if (AuthenticationService.Instance.PlayerId.Equals(checkId)) return;
            
            // If playerId is not match, save playerId in saveData as string.Empty and return false
            // Debug.Log($"Player id did not match: {checkId}");
            _saveSystem.SavePlayerId("");
        }
    }
}