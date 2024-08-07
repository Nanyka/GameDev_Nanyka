using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "Vector3Storage", menuName = "TheAiAlchemist/Storages/Vector3Storage")]
    public class Vector3Storage : ScriptableObject
    {
        private Vector3 value;
        
        public void SetValue(Vector3 value)
        {
            this.value = value;
        }

        public Vector3 GetValue()
        {
            return value;
        }
    }
}