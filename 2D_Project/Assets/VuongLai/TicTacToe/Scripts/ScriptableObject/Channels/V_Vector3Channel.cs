using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName ="V_Vector3Channel",menuName ="ScriptableObject/Channel/V_Vector3Channel")]
    public class V_Vector3Channel : ScriptableObject
    {
        private UnityEvent<Vector3> value;

        public void AddListener(UnityAction<Vector3> action)
        {
            value.AddListener(action);
        }

        public void RemoveListener(UnityAction<Vector3> action)
        {
            value.RemoveListener(action);
        }

        public void RunVector3Channel(Vector3 vector3Value)
        {
            value?.Invoke(vector3Value);
        }
    }
}