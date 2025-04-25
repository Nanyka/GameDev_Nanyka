using AlphaZeroAlgorithm;
using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "MoveChannel", menuName = "TheAiAlchemist/Channels/MoveChannel")]
    public class MoveChannel : ScriptableObject
    {
        private UnityEvent<Move> channel = new();
        
        public void AddListener(UnityAction<Move> action)
        {
            channel.AddListener(action);
        }

        public void RemoveListener(UnityAction<Move> action)
        {
            channel.RemoveListener(action);
        }

        public void ExecuteChannel(Move value)
        {
            channel?.Invoke(value);
        }
    }
}