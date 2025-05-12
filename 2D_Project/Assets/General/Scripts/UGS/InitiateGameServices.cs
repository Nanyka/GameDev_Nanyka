using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    /// <summary>
    /// This class stand out as the first point of connecting to Unity Game Servicess
    /// </summary>
    
    public class InitiateGameServices: MonoBehaviour
    {
        // [SerializeField] private CloudSaveManagerSO _cloudSaveManager;
        // [SerializeField] private AddressableManagerSO addressableManager;
        [SerializeField] private RemoteConfigManagerSO remoteConfigManager;
        [SerializeField] private BooleanStorage ugsInitialized;

        public async void Start()
        {
            await Init();
        }

        public async Task Init()
        {
            try
            {
                if (ugsInitialized.GetValue())
                    return;

                var options = new InitializationOptions();
                options.SetEnvironmentName("production");
                await UnityServices.InitializeAsync(options);
                if (this == null)
                    return;

                AnalyticsService.Instance.StartDataCollection();
                if (this == null)
                    return;

                await remoteConfigManager.FetchRemoteConfigs();
                if (this == null)
                    return;
                
                //
                // if (await SignIn())
                //     return;
                //
                // if (this == null)
                //     return;

                // await _cloudCodeManager.CheckPlayerWallet();
                // if (this == null)
                //     return;
                
                ugsInitialized.SetValue(true);
            }
            // catch (AuthenticationException ex)
            // {
            //     // Debug.LogException(ex);
            //     Debug.Log(
            //         $"The playerID has been removed. Token exist: {AuthenticationService.Instance.SessionTokenExists}");
            //     await Init();
            // }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
                ugsInitialized.SetValue(false);
            }
        }

        // private async Task<bool> SignIn()
        // {
        //     if (AuthenticationService.Instance.SessionTokenExists)
        //     {
        //         if (!AuthenticationService.Instance.IsSignedIn)
        //         {
        //             await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //             if (this == null)
        //                 return true;
        //         }
        //
        //         Debug.Log($"Player id when token exist:{AuthenticationService.Instance.PlayerId}");
        //     }
        //     else
        //     {
        //         Debug.Log("The playerId still not exist");
        //         await AuthenticationService.Instance.SignInAnonymouslyAsync();
        //
        //         if (this == null)
        //             return true;
        //         Debug.Log($"Player id new one:{AuthenticationService.Instance.PlayerId}");
        //     }
        //
        //     return false;
        // }
    }
}