using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Nana
{
    [CreateAssetMenu(fileName = "VoidChannel", menuName = "Nana/Channels/VoidChannel")]
    public class VoidChannel : ScriptableObject
    {
        private UnityEvent channel;
        
        public void AddListener(UnityAction action)
        {
            channel.AddListener(action);
        }

        public void RemoveListener(UnityAction action)
        {
            channel.RemoveListener(action);
        }

        public void ExecuteChannel()
        {
            channel?.Invoke();
        }
    }
}
