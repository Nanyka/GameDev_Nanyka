using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_IntegerChannel", menuName = "ScriptableObject/Channel/V_IntegerChannel")]
    public class V_IntegerChannel : ScriptableObject
    {
        private UnityEvent<int> channel;

        public void AddListener(UnityAction<int> unityAction)
        {
            channel.AddListener(unityAction);
        }

        public void RemoveListener(UnityAction<int> unityAction)
        {
            channel.RemoveListener(unityAction);
        }

        public void RunIntegerChannel(int inputValue)
        {
            channel?.Invoke(inputValue);
        }
    }
}
