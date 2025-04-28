using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class InGameSettings : MonoBehaviour
    {
        [Header("Setting channels")]
        [SerializeField] private LoadEventChannel loadMenu;
        [SerializeField] private GameSceneSO menuToLoad;

        [Header("UI elements")]
        [SerializeField] Button homeButton;

        private void OnEnable()
        {
            homeButton.onClick.AddListener(OnBackToHome);
        }

        private void OnDisable()
        {
            homeButton.onClick.AddListener(OnBackToHome);
        }
        
        private void OnBackToHome()
        {
            loadMenu.RaiseEvent(menuToLoad,true,true);
        }
    }
}