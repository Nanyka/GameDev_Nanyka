using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_Vector2Storage", menuName = "ScriptableObject/Storage/V_Vector2Storage")]
    public class V_Vector2Storage : ScriptableObject
    {
        private Vector2 _value = Vector2.zero;

        public Vector2 Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }
    }
}