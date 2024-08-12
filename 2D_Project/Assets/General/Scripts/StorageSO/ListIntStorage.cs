using System.Collections.Generic;
using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "ListIntStorage", menuName = "TheAiAlchemist/Storages/ListIntStorage")]
    public class ListIntStorage : ScriptableObject
    {
        [SerializeField] private List<int> value;
        [SerializeField] private int listSize = 9;

        private void OnEnable()
        {
            value = new List<int>(listSize);
            for (int i = 0; i < listSize; i++)
                value.Add(0);
        }

        public void SetValue(List<int> value)
        {
            this.value = value;
        }

        public List<int> GetValue()
        {
            return value;
        }

        public void ResetList()
        {
            for (int i = 0; i < listSize; i++)
                value[i] = 0;
        }
    }
}