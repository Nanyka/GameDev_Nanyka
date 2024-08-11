using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class CheckItem : MonoBehaviour, ICheckItemStatus
    {
        private ISpriteRendererStatus itemRender;

        private void Awake()
        {
            itemRender = GetComponent<ISpriteRendererStatus>();
        }

        public void Init(Vector3 itemPosition)
        {
            transform.position = itemPosition;
        }

        public void SetShowItem(bool showItem)
        {
            if(showItem)
            {
                itemRender.Show();
            }
            else
            {
                itemRender.Hide();
            }
        }
    }
}