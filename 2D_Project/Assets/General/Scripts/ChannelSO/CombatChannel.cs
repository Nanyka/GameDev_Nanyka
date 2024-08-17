using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "CombatChannel", menuName = "TheAiAlchemist/Channels/CombatChannel")]
    public class CombatChannel : ScriptableObject
    {
        private UnityEvent<int,bool> channel = new();
        
        public void AddListener(UnityAction<int,bool> action)
        {
            channel.AddListener(action);
        }

        public void RemoveListener(UnityAction<int,bool> action)
        {
            channel.RemoveListener(action);
        }

        public void ExecuteChannel(int value, bool isAttack)
        {
            channel?.Invoke(value,isAttack);
        }
    }
}