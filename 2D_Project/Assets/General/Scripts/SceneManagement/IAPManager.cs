using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;

namespace TheAiAlchemist
{
    public class IAPManager : MonoBehaviour, IStoreListener
    {
        private static IAPManager _instance;
        public static IAPManager Instance => _instance;

        private IStoreController m_StoreController;
        private IExtensionProvider m_StoreExtensions;

        // Your product IDs
        private const string k_ProductID_Test = "com.nanykalab.zerotictactoe.unlock";

        async void Awake()
        {
            // Singleton pattern so we only init once
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            await InitializePurchasing();
        }

        public async Task InitializePurchasing()
        {
            var options = new InitializationOptions();
            options.SetEnvironmentName("production");
            await UnityServices.InitializeAsync(options);
            if (this == null)
                return;
            
            if (IsInitialized()) return;

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct(k_ProductID_Test, ProductType.NonConsumable);
            UnityPurchasing.Initialize(this, builder);
            Debug.Log("Unity IAP: Initialization started");
        }

        public bool IsInitialized()
        {
            return m_StoreController != null && m_StoreExtensions != null;
        }

        // Call this from your UI button
        public void UnlockBots()
        {
            if (!IsInitialized())
            {
                Debug.LogError("Purchase failed because Purchasing was not initialized correctly");
                return;
            }

            m_StoreController.InitiatePurchase(k_ProductID_Test);
        }

        // IStoreListener → success
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("Unity IAP: Initialized successfully");
            m_StoreController = controller;
            m_StoreExtensions = extensions;
        }

        // IStoreListener → failure
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"Unity IAP: Initialization failed – {error}");
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
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

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Debug.Log($"Unity IAP: Purchase OK – {args.purchasedProduct.definition.id}");
            // Grant coins here…
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
        {
            Debug.LogError($"Unity IAP: Purchase failed – {product.definition.id}, {reason}");
        }
    }
}