using System.Threading.Tasks;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace TheAiAlchemist
{
    /// <summary>
    /// This class connect Remote Config service with the game
    /// </summary>
    
    [CreateAssetMenu(fileName = "AddressableManager", menuName = "TheAiAlchemist/BackEnd/Addressable Manager")]
    public class AddressableManagerSO : ScriptableObject
    {
        public async Task<Sprite> GetSprite(string objectKey)
        {
            var handle = Addressables.LoadAssetAsync<Sprite>(objectKey);
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Sprite loaded successfully!");
                return handle.Result;
            }

            Debug.LogError($"Failed to load sprite: {handle.OperationException}");
            return null;
            // return handle.WaitForCompletion();
        }
        
        public async Task<Sprite[]> GetSprites(string objectKey)
        {
            var handle = Addressables.LoadAssetAsync<Sprite[]>(objectKey);
            
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Sprites loaded successfully!");
                return handle.Result;
            }

            Debug.LogError($"Failed to load sprites: {handle.OperationException}");
            return null;
            
            // return handle.WaitForCompletion();
        }
        
        public async Task<ModelAsset> GetModel(string objectKey)
        {
            var handle = Addressables.LoadAssetAsync<ModelAsset>(objectKey);
            
            await handle.Task;
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                // Debug.Log("AI model loaded successfully!");
                return handle.Result;
            }

            Debug.LogError($"Failed to load AI model: {handle.OperationException}");
            return null;
        }
    }
}