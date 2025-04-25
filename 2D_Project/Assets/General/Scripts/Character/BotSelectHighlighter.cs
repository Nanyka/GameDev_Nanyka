using TMPro;
using UnityEngine;

namespace TheAiAlchemist
{
    public class BotSelectHighlighter : MonoBehaviour, ISelectionInteract
    {
        [SerializeField] private TextMeshProUGUI indexText;

        public void OnUpdateText(string text)
        {
            indexText.text = text;
        }

        public void OnActivate()
        {
            
        }

        public void OnDeactivate()
        {
            
        }
    }
}