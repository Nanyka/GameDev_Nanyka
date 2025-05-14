using System.Collections.Generic;
using System.Linq;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

namespace TheAiAlchemist
{
    public class IAPChecker : MonoBehaviour, IStoreListener
    {
        private IStoreController m_StoreController;
        private IExtensionProvider m_Extensions;
        private string productId = "com.nanykalab.zerotictactoe.unlock";

        private async void Start()
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
            if (this == null)
                return;

            if (m_StoreController == null)
                InitializePurchasing();
        }

        public void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add your products here
            // builder.AddProduct("unlock_all_ai_bots", ProductType.NonConsumable);
            builder.AddProduct(productId, ProductType.NonConsumable);

            UnityPurchasing.Initialize(this, builder);
        }

        // IStoreListener ▶ Successfully initialized
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log($"It is Bin Lai Log: Found {controller.products.all.Length} total products.");
            foreach (var p in controller.products.all)
            {
                Debug.Log($"  – {p.definition.id}: available={p.availableToPurchase}");
            }

            m_StoreController = controller;
            m_Extensions = extensions;
        }
        
        public void UnlockBots()
        {
            if (!IsInitialized())
            {
                Debug.LogError("Purchase failed because Purchasing was not initialized correctly");
                return;
            }

            m_StoreController.InitiatePurchase(productId);
        }
        
        public bool IsInitialized()
        {
            return m_StoreController != null && m_Extensions != null;
        }

        // IStoreListener ▶ Initialization failed
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"IAP initialization failed: {error}");

            // Gather invalid IDs by checking which products never came back availableToPurchase
            if (m_StoreController != null)
            {
                var invalidIds = m_StoreController
                    .products
                    .all
                    .Where(p => !p.availableToPurchase)
                    .Select(p => p.definition.id)
                    .ToArray();

                Debug.LogError("Invalid product IDs: " + string.Join(", ", invalidIds));
            }
            else
            {
                Debug.LogError("No store controller available yet – did InitializePurchasing() run?");
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"It is Bin Lai Log: {message}");
            Debug.LogError($"IAP initialization failed: {error}");

            // Gather invalid IDs by checking which products never came back availableToPurchase
            if (m_StoreController != null)
            {
                var invalidIds = m_StoreController
                    .products
                    .all
                    .Where(p => !p.availableToPurchase)
                    .Select(p => p.definition.id)
                    .ToArray();

                Debug.LogError("Invalid product IDs: " + string.Join(", ", invalidIds));
            }
            else
            {
                Debug.LogError("No store controller available yet – did InitializePurchasing() run?");
            }
        }

        // IStoreListener ▶ Purchase callbacks (no changes needed here)
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            /* … */
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            /* … */
        }
    }
}