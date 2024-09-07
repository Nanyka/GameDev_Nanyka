using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName ="V_IntegerStorage",menuName ="ScriptableObject/Storage/V_IntegerStorage")]
    public class V_IntegerStorage : ScriptableObject
    {
        [SerializeField] private int _value;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }
    }
}