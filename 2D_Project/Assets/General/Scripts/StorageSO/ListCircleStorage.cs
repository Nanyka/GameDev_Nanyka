using System.Collections.Generic;
using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "ListIntStorage", menuName = "TheAiAlchemist/Storages/ListIntStorage")]
    public class ListCircleStorage : ScriptableObject
    {
        [SerializeField] private int listSize = 9;
        
        private List<ICircleTrait> value;

        private void OnEnable()
        {
            value = new List<ICircleTrait>(listSize);
            for (int i = 0; i < listSize; i++)
                value.Add(null);
        }

        public void SetValue(List<ICircleTrait> value)
        {
            this.value = value;
        }

        public List<ICircleTrait> GetValue()
        {
            return value;
        }

        public void ResetList()
        {
            for (int i = 0; i < listSize; i++)
                value[i] = null;
        }
    }
}