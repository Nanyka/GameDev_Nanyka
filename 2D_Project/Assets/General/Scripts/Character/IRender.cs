using UnityEngine;

namespace TheAiAlchemist
{
    public interface IRender
    {
        public void ActivateRenderer(bool isActive);
        public void ChangeColor(Color color);
        public void SetSprite(Sprite sprite);
    }
}