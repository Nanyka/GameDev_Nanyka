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
        [SerializeField] private GameStateStorage gameStateStorage;
        [SerializeField] private Player playerFaction;

        private IRender mRenderer;

        private void Awake()
        {
            mRenderer = GetComponent<IRender>();
        }

        public void Visualize(int strength, Player player)
        {
            mRenderer.ActivateRenderer(true);
            var sprites = player == playerFaction
                ? generalAssetLoader.blueUnitSprites
                : generalAssetLoader.redUnitSprites;
            mRenderer.SetSprite(sprites[strength - 1]);
            mRenderer.ChangeMaterial(generalAssetLoader.UnlitMaterial);
        }

        public void Highlight()
        {
            mRenderer.ChangeMaterial(generalAssetLoader.LitMaterial);
        }

        public void Disable()
        {
            mRenderer.ActivateRenderer(false);
            // strengthText.text = "";
        }
    }
}