using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "CircleChannel", menuName = "TheAiAlchemist/Channels/CircleChannel")]
    public class CircleChannel : ScriptableObject
    {
        private UnityEvent<ICircleTrait> channel = new();
        
        public void AddListener(UnityAction<ICircleTrait> circleTrait)
        {
            channel.AddListener(circleTrait);
        }

        public void RemoveListener(UnityAction<ICircleTrait> circleTrait)
        {
            channel.RemoveListener(circleTrait);
        }

        public void ExecuteChannel(ICircleTrait circleTrait)
        {
            channel?.Invoke(circleTrait);
        }
    }
}