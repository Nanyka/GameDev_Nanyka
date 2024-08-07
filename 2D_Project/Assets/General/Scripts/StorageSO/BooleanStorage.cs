using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "BooleanStorage", menuName = "TheAiAlchemist/Storages/BooleanStorage")]
    public class BooleanStorage : ScriptableObject
    {
        private bool value;
        
        public void SetValue(bool value)
        {
            this.value = value;
        }

        public bool GetValue()
        {
            return value;
        }
    }    
}
