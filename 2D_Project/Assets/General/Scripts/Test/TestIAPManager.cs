using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class TestIAPManager : MonoBehaviour
    {
        [SerializeField] private IAPManager iapManager;
        [SerializeField] private UnityEvent<Product> onPurchaseCompleted;
        [SerializeField] private UnityEvent<Product, PurchaseFailureDescription> onPurchaseFailed;

        private void OnEnable()
        {
            onPurchaseCompleted.AddListener(PurchaseCompleted);
            onPurchaseFailed.AddListener(PurchaseFailed);
        }

        private void OnDisable()
        {
            onPurchaseCompleted.RemoveListener(PurchaseCompleted);
            onPurchaseFailed.RemoveListener(PurchaseFailed);
        }

        private void Start()
        {
            iapManager.SetUpPurchaseEvents(onPurchaseCompleted, onPurchaseFailed);
            iapManager.Init();
        }

        public void OnClickPurchase(string productId)
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