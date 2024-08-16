using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName ="V_Vector2Channel",menuName ="ScriptableObject/Channel/V_Vector2Channel")]
    public class V_Vector2Channel : ScriptableObject
    {
        private UnityEvent<Vector2> _value;

        public void AddListener(UnityAction<Vector2> unityAction)
        {
            _value.AddListener(unityAction);
        }

        public void RemoveListener(UnityAction<Vector2> unityAction)
        {
            _value.RemoveListener(unityAction);
        }

        public void RunVector2Channel(Vector2 input)
        {
            _value?.Invoke(input);
        }
    }
}