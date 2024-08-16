using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName ="V_MatrixPositionStorage",menuName = "ScriptableObject/Storage/V_MatrixPositionStorage")]
    public class V_MatrixPositionStorage : ScriptableObject
    {
        private V_MatrixPositionStorage _value;

        public V_MatrixPositionStorage Value
        {
            get => _value;
            set => _value = value;
        }
    }
}

