using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class PurchaseButton : MonoBehaviour
    {
        [SerializeField] private string productId;
        [SerializeField] private IAPManager iapManager;
        [SerializeField] private UnityEvent<Product> onPurchaseCompleted;
        [SerializeField] private UnityEvent<Product, PurchaseFailureDescription> onPurchaseFailed;
        [SerializeField] private Button purchaseButton;

        private void OnEnable()
        {
            // onPurchaseCompleted.AddListener(PurchaseCompleted);
            // onPurchaseFailed.AddListener(PurchaseFailed);
            purchaseButton.onClick.AddListener(OnClickPurchase);
        }

        private void OnDisable()
        {
            // onPurchaseCompleted.RemoveListener(PurchaseCompleted);
            // onPurchaseFailed.RemoveListener(PurchaseFailed);
            purchaseButton.onClick.RemoveListener(OnClickPurchase);
        }

        private void Start()
        {
            iapManager.SetUpPurchaseEvents(onPurchaseCompleted, onPurchaseFailed);
            iapManager.Init();
        }

        public void OnClickPurchase()
        {
            iapManager.Purchase(productId);
        }
        
        private void PurchaseCompleted(Product product)
        {
            Debug.Log($"Purchased product {product.definition.id} with receipt {product.receipt}");
        }
        
        private void PurchaseFailed(Product product, PurchaseFailureDescription failureReason)
        {
            Debug.Log($"Purchased product {product.definition.id} failed with {failureReason.message}");
        }
    }
}