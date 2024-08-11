using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName ="V_IntegerStorage",menuName ="ScriptableObject/Storage/V_IntegerStorage")]
    public class V_IntegerStorage : ScriptableObject
    {
        private int value;

        public void SetValue(int setValue)
        {
            value = setValue;
        }

        public int GetValue()
        {
            return value;
        }
    }
}