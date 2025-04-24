using AlphaZeroAlgorithm;
using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "PointChannel", menuName = "TheAiAlchemist/Channels/PointChannel")]
    public class PointChannel : ScriptableObject
    {
        private UnityEvent<Point> channel = new();
        
        public void AddListener(UnityAction<Point> action)
        {
            channel.AddListener(action);
        }

        public void RemoveListener(UnityAction<Point> action)
        {
            channel.RemoveListener(action);
        }

        public void ExecuteChannel(Point input)
        {
            channel?.Invoke(input);
        }
    }
}