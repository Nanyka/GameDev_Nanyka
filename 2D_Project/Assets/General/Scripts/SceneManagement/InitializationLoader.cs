using TheAiAlchemist;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
	/// <summary>
	/// This class is responsible for starting the game by loading the persistent managers scene 
	/// and raising the event to load the Main Menu
	/// </summary>

	public class InitializationLoader : MonoBehaviour
	{
		[SerializeField] private GameSceneSO _managersScene = default;
		[SerializeField] private GameSceneSO _menuToLoad = default;

		[FormerlySerializedAs("_menuLoadChannel")]
		[Header("Broadcasting on")]
		[SerializeField] private AssetReference _loadMenuChannel = default;

		private void Start()
		{
			//Load the persistent managers scene
			_managersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
		}

		private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
		{
			_loadMenuChannel.LoadAssetAsync<LoadEventChannel>().Completed += LoadMainMenu;
		}

		private void LoadMainMenu(AsyncOperationHandle<LoadEventChannel> obj)
		{
			obj.Result.RaiseEvent(_menuToLoad, true);
			SceneManager.UnloadSceneAsync(0); //Initialization is the only scene in BuildSettings, thus it has index 0
		}
	}
}

