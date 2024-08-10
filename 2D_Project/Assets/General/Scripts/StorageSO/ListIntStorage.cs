using System.Collections.Generic;
using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "ListIntStorage", menuName = "TheAiAlchemist/Storages/ListIntStorage")]
    public class ListIntStorage : ScriptableObject
    {
        [SerializeField] private List<int> value;

        private void OnEnable()
        {
            value = new List<int>();
        }

        public void SetValue(List<int> value)
        {
            this.value = value;
        }

        public List<int> GetValue()
        {
            return value;
        }
    }
}