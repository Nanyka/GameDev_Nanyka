using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "IPlayerInfoStorage", menuName = "TheAiAlchemist/Storages/IPlayerInfoStorage")]
    public class IPlayerInfoStorage : ScriptableObject
    {
        private IPlayerInfoProvider value;
        
        public void SetValue(IPlayerInfoProvider value)
        {
            this.value = value;
        }

        public IPlayerInfoProvider GetValue()
        {
            return value;
        }
    }
}