using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_BooleanStorage", menuName = "ScriptableObject/Storage/V_BooleanStorage")]
    public class V_BooleanStorage : ScriptableObject
    {
        [SerializeField] private bool value;

        public void SetValue(bool value)
        {
            this.value = value;
        }

        public bool GetValue()
        {
            return value;
        }
    }
}