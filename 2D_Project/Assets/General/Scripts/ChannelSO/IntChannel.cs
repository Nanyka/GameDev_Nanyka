using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "IntChannel", menuName = "TheAiAlchemist/Channels/IntChannel")]
    public class IntChannel : ScriptableObject
    {
        private UnityEvent<int> channel = new();
        
        public void AddListener(UnityAction<int> action)
        {
            channel.AddListener(action);
        }

        public void RemoveListener(UnityAction<int> action)
        {
            channel.RemoveListener(action);
        }

        public void ExecuteChannel(int value)
        {
            channel?.Invoke(value);
        }
    }
}