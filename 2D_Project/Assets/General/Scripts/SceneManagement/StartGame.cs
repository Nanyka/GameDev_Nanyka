using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [SerializeField] private AddressableManagerSO addressableManagerSo;
        [SerializeField] private GeneralAssetLoader generalAssetLoader;
        [SerializeField] private bool showLoadScreen;

        [Header("Broadcasting on")] 
        [SerializeField] private LoadEventChannel loadLocation;
        [SerializeField] private LoadEventChannel loadTutorial;

        [Header("Listening to")] 
        [SerializeField] private VoidChannel onNewGameButton;
        [SerializeField] private VoidChannel onTutorialButton;
        [SerializeField] private VoidChannel onResetButton;
        // [SerializeField] private bool hasSaveData;

        private async void Start()
        {
            // hasSaveData = saveSystem.LoadSaveDataFromDisk();
            await LoadGeneralElements();
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
        
        private async Task LoadGeneralElements()
        {
            var blueUnitSprites = new List<Sprite>(3);
            foreach (var unitAddress in generalAssetLoader.blueUnitAddress)
            {
                var sprite = await addressableManagerSo.GetSprite(unitAddress);
                blueUnitSprites.Add(sprite);
            }
            generalAssetLoader.ResetBlueSprites(blueUnitSprites);
            
            var redUnitSprites = new List<Sprite>(3);
            foreach (var unitAddress in generalAssetLoader.redUnitAddress)
            {
                var sprite = await addressableManagerSo.GetSprite(unitAddress);
                redUnitSprites.Add(sprite);
            }
            generalAssetLoader.ResetRedSprites(redUnitSprites);
            
            var remainSprites = new List<Sprite>(3);
            foreach (var remainAddress in generalAssetLoader.remainAmountAddress)
            {
                var sprite = await addressableManagerSo.GetSprite(remainAddress);
                remainSprites.Add(sprite);
            }
            generalAssetLoader.ResetRemainSprites(remainSprites);
        }
    }
}