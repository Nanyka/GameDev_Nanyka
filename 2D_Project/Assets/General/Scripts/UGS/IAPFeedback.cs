using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace TheAiAlchemist
{
    [RequireComponent(typeof(IAPInvoker))]
    public class IAPFeedback : MonoBehaviour
    {
        [SerializeField] private SaveSystemManager saveSystemManager;
        
        private IAPInvoker _iapInvoker;

        private void Awake()
        {
            _iapInvoker = GetComponent<IAPInvoker>();
        }

        public async void OnPurchaseCompleted(Product product)
        {
            // Generate an account
            // Save playerId into saveData and return true
            if (AuthenticationService.Instance.SessionTokenExists)
            {
                if (!AuthenticationService.Instance.IsSignedIn)
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();

                Debug.Log($"Player id when token exist:{AuthenticationService.Instance.PlayerId}");
            }
            else
            {
                Debug.Log("The playerId still not exist");
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"Player id new one:{AuthenticationService.Instance.PlayerId}");
            }

            saveSystemManager.SavePlayerId(AuthenticationService.Instance.PlayerId);
            _iapInvoker.ListenIapFeedback(true);
            Debug.Log($"PurchaseCompleted: {product.definition.id}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription reason)
        {
            // Return false
            _iapInvoker.ListenIapFeedback(false);
            Debug.Log($"PurchaseFailed: {reason.message}");
            
        }
    }
}
