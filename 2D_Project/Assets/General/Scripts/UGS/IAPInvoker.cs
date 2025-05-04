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
        [SerializeField] private TextMeshProUGUI iapText;
        [SerializeField] private GameObject unlockButton;
        [SerializeField] private GameObject thanksButton;
        
        private bool _isPurchased = false;

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
            // Debug.Log(saveSystem.saveData.playerId);
            if (saveSystem.saveData.playerId != "")
            {
                ListenIapFeedback(true);
                iapStateChannel.ExecuteChannel(true);
                iapPanel.SetActive(false);
                return;
            }
            
            // Debug.LogWarning("No player ID specified");
            
            // If not, check internet availability

            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                internetText.SetActive(true);
                iapStateChannel.ExecuteChannel(false);
                return;
            }
            internetText.SetActive(false);
            // Debug.Log("Network reachability is reachable");

            // Show the IAP panel
            iapText.text = "If you’re enjoying it, consider buying me a coffee to support the development!" +
                           "\nThanks so much! 😊";
            unlockButton.SetActive(true);
            thanksButton.SetActive(false);
            iapPanel.SetActive(true);
        }
        
        public void ListenIapFeedback(bool isPurchased)
        {
            _isPurchased = isPurchased;
            iapText.text = _isPurchased ? "Thanks a ton for the coffee!\nIt keeps both me and the game running! 😊" :
                "Thanks so much for playing!\n Your support means a lot.";
            unlockButton.SetActive(false);
            thanksButton.SetActive(true);
            
            // iapPanel.SetActive(false);
            // iapStateChannel.ExecuteChannel(isPurchased);
        }

        public void OnClickThanks()
        {
            iapPanel.SetActive(false);
            iapStateChannel.ExecuteChannel(_isPurchased);
        }
    }
}