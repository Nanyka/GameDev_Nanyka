using System;
using TMPro;
using UnityEngine;

namespace TheAiAlchemist
{
    public class PlayerSelectHighlighter : MonoBehaviour, ISelectionInteract
    {
        [SerializeField] private VoidChannel changePlayerChannel;
        
        [SerializeField] private TextMeshProUGUI indexText;
        [SerializeField] private GameObject highlighter;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(OnDeactivate);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(OnDeactivate);
        }

        public void OnUpdateText(string text)
        {
            indexText.text = text;
        }

        public void OnActivate()
        {
            highlighter.SetActive(true);
        }

        public void OnDeactivate()
        {
            highlighter.SetActive(false);
        }
    }
}