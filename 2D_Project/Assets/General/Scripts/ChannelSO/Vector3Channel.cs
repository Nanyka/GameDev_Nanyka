using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "Vector3Channel", menuName = "TheAiAlchemist/Channels/Vector3Channel")]
    public class Vector3Channel : ScriptableObject
    {
        private UnityEvent<Vector3> channel;
        
        public void AddListener(UnityAction<Vector3> action)
        {
            channel.AddListener(action);
        }

        public void RemoveListener(UnityAction<Vector3> action)
        {
            channel.RemoveListener(action);
        }

        public void ExecuteChannel(Vector3 input)
        {
            channel?.Invoke(input);
        }
    }
}