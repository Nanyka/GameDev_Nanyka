using System;
using UnityEngine;

namespace TheAiAlchemist
{
    public class RenderComp : MonoBehaviour,IRender
    {
        [SerializeField] private SpriteRenderer m_Renderer;

        public void ActivateRenderer(bool isActive)
        {
            m_Renderer.enabled = isActive;
        }
    }
}