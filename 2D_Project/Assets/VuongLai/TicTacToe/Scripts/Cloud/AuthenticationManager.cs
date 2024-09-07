using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core.Environments;

namespace V_TicTacToe
{
    public class AuthenticationManager : MonoBehaviour
    {
        [SerializeField] private V_VoidChannel authenticationSuccessChannel;

        private void Start()
        {
            var option = new InitializationOptions();
            option.SetEnvironmentName("vuong_dev");
            UnityServices.InitializeAsync(option);

            SignIn();
        }

        private async void SignIn()
        {
            await AnonymousSignIn();
        }

        private async Task AnonymousSignIn()
        {
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                try
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();

                    Debug.Log($"Sign In Success, Player Id: {AuthenticationService.Instance.PlayerId}");

                    authenticationSuccessChannel.RunVoidChannel();
                }
                catch (AuthenticationException ex)
                {
                    Debug.LogException(ex);
                }
                catch (RequestFailedException failEx)
                {
                    Debug.LogException(failEx);
                }
            }
        }
    }
}
