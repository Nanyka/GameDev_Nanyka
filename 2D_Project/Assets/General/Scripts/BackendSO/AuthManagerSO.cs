using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace TheAiAlchemist
{
    /// <summary>
    /// This class manage authentication flow
    /// </summary>
    
    [CreateAssetMenu(fileName = "AuthenticationManager", menuName = "TheAiAlchemist/BackEnd/AuthenticationManager")]
    public class AuthManagerSO: ScriptableObject
    {
        #region ANONYMOUS

        public bool CheckPlayerId(string checkId)
        {
            return AuthenticationService.Instance.PlayerId.Equals(checkId);
        }

        public async Task<bool> AnonymousSignIn()
        {
            if (AuthenticationService.Instance.SessionTokenExists)
            {
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    if (this == null)
                        return true;
                }

                Debug.Log($"Player id when token exist:{AuthenticationService.Instance.PlayerId}");
            }
            else
            {
                Debug.Log("The playerId still not exist");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (this == null)
                    return true;
                Debug.Log($"Player id new one:{AuthenticationService.Instance.PlayerId}");
            }

            return false;
        }
        
        public async Task<bool> AddOrSignUpWithPassAsync(string username, string password)
        {
            if (AuthenticationService.Instance.SessionTokenExists)
            {
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    await AuthenticationService.Instance.AddUsernamePasswordAsync(username, password);
                }
                Debug.Log("Username and password added.");
                if (this == null)
                    return true;
            }
            else
            {
                Debug.Log("The playerId still not exist");
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);

                if (this == null)
                    return true;
                Debug.Log($"Player id new one:{AuthenticationService.Instance.PlayerId}");
            }

            return false;
        }

        #endregion

        #region USERNAME & PASSWORD

        public async Task SignUpWithUsernamePasswordAsync(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
                Debug.Log("SignUp is successful.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }
        
        public async Task SignInWithUsernamePasswordAsync(string username, string password)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
                Debug.Log($"User: {AuthenticationService.Instance.PlayerId}\nSignIn is successful.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }

        #endregion
        
    }
}