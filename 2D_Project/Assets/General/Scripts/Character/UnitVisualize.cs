using System;
using AlphaZeroAlgorithm;
using TMPro;
using UnityEngine;

namespace TheAiAlchemist
{
    public class UnitVisualize : MonoBehaviour
    {
        // [SerializeField] private TextMeshProUGUI strengthText;
        [SerializeField] private GeneralAssetLoader generalAssetLoader;

        private IRender mRenderer;

        private void Awake()
        {
            mRenderer = GetComponent<IRender>();
        }

        public void Visualize(int strength, Player player)
        {
            mRenderer.ActivateRenderer(true);
            var sprites = player == Player.X ? generalAssetLoader.blueUnitSprites : generalAssetLoader.redUnitSprites;
            mRenderer.SetSprite(sprites[strength - 1]);
        }

        // public void Visualize(int strength, Color color)
        // {
        //     mRenderer.ActivateRenderer(true);
        //     // mRenderer.ChangeColor(color);
        //     // strengthText.text = strength.ToString();
        // }

        public void Disable()
        {
            mRenderer.ActivateRenderer(false);
            // strengthText.text = "";
        }
    }
}