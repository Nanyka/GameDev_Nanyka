using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_VoidChannel", menuName = "ScriptableObject/Channel/V_VoidChannel")]
    public class V_VoidChannel : ScriptableObject
    {
        private UnityEvent value;

        public void AddListener(UnityAction action)
        {
            value.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            value.RemoveListener(action);
        }

        public void RunVoidChannel()
        {
            value?.Invoke();
        }
    }
}