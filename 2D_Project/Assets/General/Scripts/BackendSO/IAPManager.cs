using System;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "IAPManager", menuName = "TheAiAlchemist/BackEnd/IAPManager")]
    public class IAPManager : ScriptableObject, IStoreListener
    {
        [SerializeField] private string[] products = { "com.nanykalab.zerotictactoe.unlock", "unlock_all_ai_bots" };

        private IStoreController _mStoreController;
        private IExtensionProvider _mExtensions;
        private UnityEvent<Product> _purchasedEvent;
        private UnityEvent<Product, PurchaseFailureDescription> _purchaseFailedEvent;
        private UnityEvent<bool, string> onRestoreCompleted;
        private UnityEvent<Product> onProductFetched;

        public void SetUpPurchaseEvents(UnityEvent<Product> purchaseCompletedEvent,
            UnityEvent<Product, PurchaseFailureDescription> purchaseFailedEvent)
        {
            _purchasedEvent = purchaseCompletedEvent;
            _purchaseFailedEvent = purchaseFailedEvent;
        }

        public void SetUpRestoreEvents(UnityEvent<bool, string> restoreCompletedEvent,
            UnityEvent<Product> productFetchedEvent)
        {
            onRestoreCompleted = restoreCompletedEvent;
            onProductFetched = productFetchedEvent;
        }

        public void Init()
        {
            if (_mStoreController == null)
                InitializePurchasing();
        }

        private void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add your products here
            // builder.AddProduct("unlock_all_ai_bots", ProductType.NonConsumable);
            foreach (var productId in products)
                builder.AddProduct(productId, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        // IStoreListener ▶ Successfully initialized
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Debug.Log($"It is Bin Lai Log: Found {controller.products.all.Length} total products.");
            // foreach (var p in controller.products.all)
            // {
            //     Debug.Log($"  – {p.definition.id}: available={p.availableToPurchase}");
            // }

            _mStoreController = controller;
            _mExtensions = extensions;
        }

        #region Purchasing button

        public void Purchase(string productId)
        {
            if (!IsInitialized())
            {
                Debug.LogError("Purchase failed because Purchasing was not initialized correctly");
                return;
            }

            _mStoreController.InitiatePurchase(productId);
        }

        #endregion

        private bool IsInitialized()
        {
            return _mStoreController != null && _mExtensions != null;
        }

        // IStoreListener ▶ Initialization failed
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Debug.LogError($"IAP initialization failed: {error}");

            // if (_mStoreController != null)
            // {
            //     var invalidIds = _mStoreController
            //         .products
            //         .all
            //         .Where(p => !p.availableToPurchase)
            //         .Select(p => p.definition.id)
            //         .ToArray();
            //
            //     Debug.LogError("Invalid product IDs: " + string.Join(", ", invalidIds));
            // }
            // else
            // {
            //     Debug.LogError("No store controller available yet – did InitializePurchasing() run?");
            // }
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            // Debug.LogError($"It is Bin Lai Log: {message}");
            // Debug.LogError($"IAP initialization failed: {error}");

            // if (_mStoreController != null)
            // {
            //     var invalidIds = _mStoreController
            //         .products
            //         .all
            //         .Where(p => !p.availableToPurchase)
            //         .Select(p => p.definition.id)
            //         .ToArray();
            //
            //     Debug.LogError("Invalid product IDs: " + string.Join(", ", invalidIds));
            // }
            // else
            // {
            //     Debug.LogError("No store controller available yet – did InitializePurchasing() run?");
            // }
        }

        // IStoreListener ▶ Purchase callbacks (no changes needed here)
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            _purchasedEvent?.Invoke(args.purchasedProduct);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            var msg = $"Purchase of {product.definition.id} failed: {reason}";
            var desc = new PurchaseFailureDescription(
                product.definition.id,
                reason,
                msg
            );

            _purchaseFailedEvent?.Invoke(product, desc);
        }

        #region Restore button

        /// <summary>
        /// Attempts to restore previously made non‐consumable purchases (iOS / macOS only).
        /// </summary>
        public void Restore()
        {
            if (!IsInitialized())
            {
                Debug.LogError("Restore failed because Purchasing was not initialized correctly");
                return;
            }

            // Only applicable on Apple platforms
#if UNITY_IOS || UNITY_STANDALONE_OSX
            Debug.Log("RestorePurchases started …");

            var apple = _mExtensions.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result,message) =>
            {
                Debug.Log($"RestorePurchases is {result} with message: {message}");
            });
#else
        Debug.LogWarning("RestorePurchases is not supported on this platform. Current = " + Application.platform);
#endif
        }

        #endregion
    }
}