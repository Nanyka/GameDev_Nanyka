using UnityEngine;

namespace TheAiAlchemist
{
    public interface IRender
    {
        public void ActivateRenderer(bool isActive);
        public void ChangeMaterial(Material material);
        public void SetSprite(Sprite sprite);
    }
}