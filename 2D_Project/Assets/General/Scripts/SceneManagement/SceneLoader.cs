﻿using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace TheAiAlchemist
{
    /// <summary>
    /// This class manages the scene loading and unloading.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private GameSceneSO _gameplayScene = default;
        [SerializeField] private GameSceneSO _tutorialScene = default;
        [SerializeField] private GameSceneSO _bossPlayScene = default;

        [Header("Listening to")] 
        [SerializeField] private LoadEventChannel _loadLocation = default;
        [SerializeField] private LoadEventChannel _loadTutorial = default;
        [SerializeField] private LoadEventChannel _loadBossGame = default;
        [SerializeField] private LoadEventChannel _loadMenu = default;
        [SerializeField] private LoadEventChannel _coldStartupLocation = default;

        [Header("Broadcasting on")] 
        [SerializeField] private BoolChannel _toggleLoadingScreen = default;
        [SerializeField] private VoidChannel _onSceneReady = default; //picked up by the SpawnSystem
        [SerializeField] private FadeChannel _fadeRequestChannel = default;
        [SerializeField] private VoidChannel _activateInputReader;
        [SerializeField] private VoidChannel _readyForGame;

        private AsyncOperationHandle<SceneInstance> _loadingOperationHandle;
        private AsyncOperationHandle<SceneInstance> _gameplayManagerLoadingOpHandle;

        //Parameters coming from scene loading requests
        private GameSceneSO _sceneToLoad;
        private GameSceneSO _currentlyLoadedScene;
        private bool _showLoadingScreen;

        private SceneInstance _gameplayManagerSceneInstance;
        private float _fadeDuration = 0.5f;
        private bool _isLoading = false; //To prevent a new loading request while already loading a new scene

        private void OnEnable()
        {
            _loadLocation.OnLoadingRequested += LoadLocation;
            _loadTutorial.OnLoadingRequested += LoadTutorial;
            _loadBossGame.OnLoadingRequested += LoadBossGame;
            _loadMenu.OnLoadingRequested += LoadMenu;
#if UNITY_EDITOR
            _coldStartupLocation.OnLoadingRequested += LocationColdStartup;
#endif
        }

        private void OnDisable()
        {
            _loadLocation.OnLoadingRequested -= LoadLocation;
            _loadTutorial.OnLoadingRequested -= LoadTutorial;
            _loadBossGame.OnLoadingRequested -= LoadBossGame;
            _loadMenu.OnLoadingRequested -= LoadMenu;
#if UNITY_EDITOR
            _coldStartupLocation.OnLoadingRequested -= LocationColdStartup;
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        /// This special loading function is only used in the editor, when the developer presses Play in a Location scene, without passing by Initialisation.
        /// </summary>
        private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen, bool fadeScreen)
        {
            _currentlyLoadedScene = currentlyOpenedLocation;

            if (_currentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
            {
                //Gameplay managers is loaded synchronously
                _gameplayManagerLoadingOpHandle =
                    _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.WaitForCompletion();
                _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

                StartGameplay();
            }
        }
#endif

        /// <summary>
        /// This function loads the location scenes passed as array parameter
        /// </summary>
        private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
            if (_isLoading)
                return;

            _sceneToLoad = locationToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            //In case we are coming from the main menu, we need to load the Gameplay manager scene first
            if (!_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOpHandle =
                    _gameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
            }
            else
            {
                StartCoroutine(UnloadPreviousScene());
            }
        }

        private void LoadTutorial(GameSceneSO tutorialToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            if (_isLoading)
                return;

            _sceneToLoad = tutorialToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            //In case we are coming from the main menu, we need to load the Gameplay manager scene first
            if (!_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOpHandle =
                    _tutorialScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
            }
            else
            {
                StartCoroutine(UnloadPreviousScene());
            }
        }
        
        /// <summary>
        /// This function loads the location scenes passed as array parameter
        /// </summary>
        private void LoadBossGame(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
            if (_isLoading)
                return;

            _sceneToLoad = locationToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            //In case we are coming from the main menu, we need to load the Gameplay manager scene first
            if (!_gameplayManagerSceneInstance.Scene.isLoaded)
            {
                _gameplayManagerLoadingOpHandle =
                    _bossPlayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
                _gameplayManagerLoadingOpHandle.Completed += OnGameplayManagersLoaded;
            }
            else
            {
                StartCoroutine(UnloadPreviousScene());
            }
        }

        private void OnGameplayManagersLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            _gameplayManagerSceneInstance = _gameplayManagerLoadingOpHandle.Result;

            StartCoroutine(UnloadPreviousScene());
        }

        /// <summary>
        /// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
        /// </summary>
        private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen, bool fadeScreen)
        {
            //Prevent a double-loading, for situations where the player falls in two Exit colliders in one frame
            if (_isLoading)
                return;

            _sceneToLoad = menuToLoad;
            _showLoadingScreen = showLoadingScreen;
            _isLoading = true;

            //In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
            if (_gameplayManagerSceneInstance.Scene.isLoaded)
                Addressables.UnloadSceneAsync(_gameplayManagerLoadingOpHandle, true);

            StartCoroutine(UnloadPreviousScene());
        }

        /// <summary>
        /// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
        /// </summary>
        private IEnumerator UnloadPreviousScene()
        {
            // _inputReader.DisableAllInput();
            _fadeRequestChannel.FadeOut(_fadeDuration / 2);

            yield return new WaitForSeconds(_fadeDuration);

            if (_currentlyLoadedScene != null) //would be null if the game was started in Initialisation
            {
                if (_currentlyLoadedScene.sceneReference.OperationHandle.IsValid())
                {
                    //Unload the scene through its AssetReference, i.e. through the Addressable system
                    _currentlyLoadedScene.sceneReference.UnLoadScene();
                }
#if UNITY_EDITOR
                else
                {
                    //Only used when, after a "cold start", the player moves to a new scene
                    //Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
                    //the scene needs to be unloaded using regular SceneManager instead of as an Addressable
                    SceneManager.UnloadSceneAsync(_currentlyLoadedScene.sceneReference.editorAsset.name);
                }
#endif
            }

            LoadNewScene();
        }

        /// <summary>
        /// Kicks off the asynchronous loading of a scene, either menu or Location.
        /// </summary>
        private void LoadNewScene()
        {
            if (_showLoadingScreen)
            {
                _toggleLoadingScreen.ExecuteChannel(true);
            }

            _loadingOperationHandle = _sceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
            _loadingOperationHandle.Completed += OnNewSceneLoaded;
        }

        private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            //Save loaded scenes (to be unloaded at next load request)
            _currentlyLoadedScene = _sceneToLoad;

            Scene s = obj.Result.Scene;
            SceneManager.SetActiveScene(s);
            LightProbes.TetrahedralizeAsync();

            _isLoading = false;

            if (_showLoadingScreen)
                _toggleLoadingScreen.ExecuteChannel(false);

            _fadeRequestChannel.FadeIn(_fadeDuration);

            StartGameplay();
        }

        private void StartGameplay()
        {
            _onSceneReady.ExecuteChannel(); //Spawn system will spawn a gameplay scene
            _activateInputReader.ExecuteChannel(); // Assign main camera on InputManager and enable InputReader
            _readyForGame.ExecuteChannel(); // Initiate game state

        }

        private void ExitGame()
        {
            Application.Quit();
            Debug.Log("Exit!");
        }
    }
}