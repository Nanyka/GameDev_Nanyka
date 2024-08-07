using UnityEngine;

namespace TheAiAlchemist
{
    public interface ICircleTrait
    {
        public void Init(Vector3 spawnPos);
        public bool DetectTouchPoint(Vector3 inputPosition);
        public int GetId();
    }
}