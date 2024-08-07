using UnityEngine;

namespace Nana
{
    [CreateAssetMenu(fileName = "BooleanStorage", menuName = "Nana/Storages/BooleanStorage")]
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
