using System;
using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "IntStorage", menuName = "TheAiAlchemist/Storages/IntStorage")]
    public class IntStorage : ScriptableObject
    {
        private int value;

        private void OnEnable()
        {
            value = 0;
        }

        public void SetValue(int value)
        {
            this.value = value;
        }

        public int GetValue()
        {
            return value;
        }
    }
}