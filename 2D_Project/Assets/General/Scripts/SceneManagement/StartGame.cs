using System.Collections;
using TheAiAlchemist;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    /// <summary>
    /// This class contains the function to call when play button is pressed
    /// </summary>
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private GameSceneSO locationsToLoad;
        [SerializeField] private GameSceneSO tutorialToLoad;
        [SerializeField] private SaveSystemManager saveSystem;
        [SerializeField] private bool showLoadScreen;

        [Header("Broadcasting on")] [SerializeField]
        private LoadEventChannel loadLocation;

        [SerializeField] private LoadEventChannel loadTutorial;

        [Header("Listening to")] 
        [SerializeField] private VoidChannel onNewGameButton;
        [SerializeField] private VoidChannel onTutorialButton;
        [SerializeField] private VoidChannel onResetButton;
        [SerializeField] private bool hasSaveData;

        private void Start()
        {
            // hasSaveData = saveSystem.LoadSaveDataFromDisk();
            onNewGameButton.AddListener(StartNewGame);
            onTutorialButton.AddListener(TutorialGame);
            onResetButton.AddListener(OnResetSaveDataPress);
        }

        private void OnDestroy()
        {
            onNewGameButton.RemoveListener(StartNewGame);
            onTutorialButton.RemoveListener(TutorialGame);
            onResetButton.RemoveListener(OnResetSaveDataPress);
        }

        private void StartNewGame()
        {
            hasSaveData = saveSystem.LoadSaveDataFromDisk();
            if (hasSaveData == false)
            {
                saveSystem.WriteEmptySaveFile();
                saveSystem.SetNewGameData();
            }

            loadLocation.RaiseEvent(locationsToLoad, showLoadScreen);
        }

        private void TutorialGame()
        {
            loadTutorial.RaiseEvent(tutorialToLoad, showLoadScreen);
        }

        private void OnResetSaveDataPress()
        {
            saveSystem.saveData.level = 0;
            saveSystem.SaveDataToDisk();
            
            loadLocation.RaiseEvent(locationsToLoad, showLoadScreen);
        }

        // private void ContinuePreviousGame()
        // {
        //     // StartCoroutine(LoadSaveGame());
        // }

        // private IEnumerator LoadSaveGame()
        // {
        //     yield return StartCoroutine(_saveSystem.LoadSavedInventory());
        //
        //     _saveSystem.LoadSavedQuestlineStatus();
        //     var locationGuid = _saveSystem.saveData._locationId;
        //     var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);
        //
        //     yield return asyncOperationHandle;
        //
        //     if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         LocationSO locationSO = asyncOperationHandle.Result;
        //         _loadLocation.RaiseEvent(locationSO, _showLoadScreen);
        //     }
        // }
    }
}