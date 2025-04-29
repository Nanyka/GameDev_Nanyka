using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class PlayerSelectHighlighter : MonoBehaviour, ISelectionInteract
    {
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private GeneralAssetLoader generalAssetLoader;
        [SerializeField] private Image remainAmountImage;
        [SerializeField] private GameObject highlighter;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(OnDeactivate);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(OnDeactivate);
        }

        public void OnUpdateRemainAmount(int remain)
        {
            remainAmountImage.sprite = generalAssetLoader.remainAmountSprites[remain];
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