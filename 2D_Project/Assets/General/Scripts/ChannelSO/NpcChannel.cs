using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "NpcChannel", menuName = "TheAiAlchemist/Channels/NpcChannel")]
    public class NpcChannel : ScriptableObject
    {
        private UnityEvent<INpcPlayer> channel = new();
        
        public void AddListener(UnityAction<INpcPlayer> action)
        {
            channel.AddListener(action);
        }

        public void RemoveListener(UnityAction<INpcPlayer> action)
        {
            channel.RemoveListener(action);
        }

        public void ExecuteChannel(INpcPlayer value)
        {
            channel?.Invoke(value);
        }
    }
}