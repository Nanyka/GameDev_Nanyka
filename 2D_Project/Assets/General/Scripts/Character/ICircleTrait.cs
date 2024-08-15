using UnityEngine;

namespace TheAiAlchemist
{
    public interface ICircleTrait
    {
        public void Init(Vector3 spawnPos, int playerId,int priority);
        public bool DetectTouchPoint(Vector3 inputPosition);
        public int GetPlayerId();
        public int GetId();
        public int GetPriority();
        public void DisableCircle();
    }
}