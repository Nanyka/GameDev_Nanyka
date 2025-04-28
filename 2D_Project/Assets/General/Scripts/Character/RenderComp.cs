using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class RenderComp : MonoBehaviour,IRender
    {
        [SerializeField] private SpriteRenderer mRenderer;

        public void ActivateRenderer(bool isActive)
        {
            mRenderer.enabled = isActive;
        }

        public void ChangeColor(Color color)
        {
            mRenderer.color = color;
        }

        public void SetSprite(Sprite sprite)
        {
            mRenderer.sprite = sprite;
        }
    }
}