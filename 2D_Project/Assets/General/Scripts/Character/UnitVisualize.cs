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
        [SerializeField] private Player playerFaction;

        private IRender mRenderer;
        private UnitDropperVfx unitDropperVfx;

        private void Awake()
        {
            mRenderer = GetComponent<IRender>();
            unitDropperVfx = GetComponent<UnitDropperVfx>();
        }

        public void Visualize(int strength, Player player, bool isBeatOpponent)
        {
            // mRenderer.ActivateRenderer(true);
            var sprites = player == playerFaction
                ? generalAssetLoader.blueUnitSprites
                : generalAssetLoader.redUnitSprites;
            // mRenderer.SetSprite(sprites[strength - 1]);
            mRenderer.ChangeMaterial(generalAssetLoader.UnlitMaterial);
            unitDropperVfx.PlayEffect(sprites[strength - 1], isBeatOpponent);
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