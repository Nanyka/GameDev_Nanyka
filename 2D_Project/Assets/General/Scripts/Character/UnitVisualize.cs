using System;
using TMPro;
using UnityEngine;

namespace TheAiAlchemist
{
    public class UnitVisualize : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI strengthText;

        private IRender mRenderer;

        private void Awake()
        {
            mRenderer = GetComponent<IRender>();
        }

        public void Visualize(int strength, Color color)
        {
            mRenderer.ActivateRenderer(true);
            mRenderer.ChangeColor(color);
            strengthText.text = strength.ToString();
        }

        public void Disable()
        {
            mRenderer.ActivateRenderer(false);
            strengthText.text = "";
        }
    }
}