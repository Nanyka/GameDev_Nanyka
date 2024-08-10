using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "TwoIntChannel", menuName = "TheAiAlchemist/Channels/TwoIntChannel")]
    public class TwoIntChannel : ScriptableObject
    {
        private UnityEvent<int,int> channel = new();
        
        public void AddListener(UnityAction<int,int> action)
        {
            channel.AddListener(action);
        }

        public void RemoveListener(UnityAction<int,int> action)
        {
            channel.RemoveListener(action);
        }

        public void ExecuteChannel(int firtItem, int secondItem)
        {
            channel?.Invoke(firtItem,secondItem);
        }
    }
}