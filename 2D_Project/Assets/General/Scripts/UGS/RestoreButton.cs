using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class RestoreButton : MonoBehaviour
    {
        [SerializeField] private IAPManager iapManager;
        [SerializeField] private UnityEvent<bool,string> onRestoreCompleted;
        [SerializeField] private UnityEvent<Product> onProductFetched;
        [SerializeField] private Button restoreButton;
        
        private void OnEnable()
        {
            restoreButton.onClick.AddListener(OnClickPurchase);
        }

        private void OnDisable()
        {
            restoreButton.onClick.RemoveListener(OnClickPurchase);
        }

        private void Start()
        {
            iapManager.SetUpRestoreEvents(onRestoreCompleted, onProductFetched);
            iapManager.Init();
        }

        public void OnClickPurchase()
        {
            iapManager.Restore();
        }
    }
}