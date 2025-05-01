using System;
using System.Collections;
using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class IAPInvoker : MonoBehaviour
    {
        [SerializeField] private SaveSystemManager saveSystem;
        [SerializeField] private VoidChannel checkIapState;

        [SerializeField] private BoolChannel iapStateChannel;
        [SerializeField] private GameObject iapPanel;
        [SerializeField] private GameObject internetText;

        private void OnEnable()
        {
            checkIapState.AddListener(CheckIAPState);
        }

        private void OnDisable()
        {
            checkIapState.RemoveListener(CheckIAPState);
        }

        private void CheckIAPState()
        {
            // Check saveData, if available, return true
            if (saveSystem.saveData.playerId != "")
            {
                Debug.Log($"Player id:{saveSystem.saveData.playerId}");
                ListenIapFeedback(true);
                return;
            }
            
            Debug.LogWarning("No player ID specified");
            
            // If not, check internet availability

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                internetText.SetActive(true);
                return;
            }
            internetText.SetActive(false);
            Debug.Log("Network reachability is reachable");

            // Show the IAP panel
            iapPanel.SetActive(true);
        }
        
        public void ListenIapFeedback(bool isPurchased)
        {
            iapPanel.SetActive(false);
            iapStateChannel.ExecuteChannel(isPurchased);
        }
    }
}