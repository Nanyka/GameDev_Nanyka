using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class CheckItemRender : MonoBehaviour, ISpriteRendererStatus
    {
        [SerializeField] private SpriteRenderer itemRender;

        public void Hide()
        {
            itemRender.enabled = false;
        }

        public void Show()
        {
            itemRender.enabled = true;
        }
    }
}

