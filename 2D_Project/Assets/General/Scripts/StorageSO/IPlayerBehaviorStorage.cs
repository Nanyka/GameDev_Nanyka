using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "IPlayerBehaviorStorage", menuName = "TheAiAlchemist/Storages/IPlayerBehaviorStorage")]
    public class IPlayerBehaviorStorage : ScriptableObject
    {
        private IPlayerBehavior value;
        
        public void SetValue(IPlayerBehavior value)
        {
            this.value = value;
        }

        public IPlayerBehavior GetValue()
        {
            return value;
        }
    }
}