using UnityEngine;

namespace TheAiAlchemist
{
    public interface IObjectPool
    {
        public GameObject GetObject();
        public void ResetPool();
    }
}