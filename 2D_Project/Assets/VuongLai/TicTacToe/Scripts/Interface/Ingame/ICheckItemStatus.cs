using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public interface ICheckItemStatus
    {
        public void Init(Vector3 itemPosition);
        public void SetShowItem(bool showItem);
    }
}