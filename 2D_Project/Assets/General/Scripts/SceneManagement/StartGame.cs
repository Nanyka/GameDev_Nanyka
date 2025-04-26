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
    // [SerializeField] private SaveSystem _saveSystem = default;
    [SerializeField] private bool showLoadScreen = default;

    [Header("Broadcasting on")] 
    [SerializeField] private LoadEventChannel loadLocation = default;
    [SerializeField] private LoadEventChannel loadTutorial = default;

    [Header("Listening to")] 
    [SerializeField] private VoidChannel onNewGameButton = default;
    [SerializeField] private VoidChannel onContinueButton = default;
    [SerializeField] private VoidChannel onTutorialButton = default;

    // [SerializeField] private bool hasSaveData;

    private void Start()
    {
        // _hasSaveData = _saveSystem.LoadSaveDataFromDisk();
        onNewGameButton.AddListener(StartNewGame);
        onContinueButton.AddListener(ContinuePreviousGame);
        onTutorialButton.AddListener(TutorialGame);
    }

    private void OnDestroy()
    {
        onNewGameButton.RemoveListener(StartNewGame);
        onContinueButton.RemoveListener(ContinuePreviousGame);
        onTutorialButton.RemoveListener(TutorialGame);
    }

    private void StartNewGame()
    {
        // hasSaveData = false;
        // _saveSystem.WriteEmptySaveFile();
        // _saveSystem.SetNewGameData();
        loadLocation.RaiseEvent(locationsToLoad, showLoadScreen);
    }
    
    private void TutorialGame()
    {
        loadTutorial.RaiseEvent(tutorialToLoad, showLoadScreen);
    }

    private void OnResetSaveDataPress()
    {
        // hasSaveData = false;
    }

    private void ContinuePreviousGame()
    {
        // StartCoroutine(LoadSaveGame());
    }

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
