using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheAiAlchemist
{
    public class BotSelectHighlighter : MonoBehaviour, ISelectionInteract
    {
        [SerializeField] private GeneralAssetLoader generalAssetLoader;
        [SerializeField] private Image remainAmountImage;

        // public void OnUpdateText(string text)
        // {
        //     indexText.text = text;
        // }

        public void OnUpdateRemainAmount(int remain)
        {
            remainAmountImage.sprite = generalAssetLoader.remainAmountSprites[remain];
        }

        public void OnActivate()
        {
            
        }

        public void OnDeactivate()
        {
            
        }
    }
}