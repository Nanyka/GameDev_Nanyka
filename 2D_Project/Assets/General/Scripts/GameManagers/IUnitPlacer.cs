using UnityEngine;

namespace TheAiAlchemist
{
    public interface IUnitPlacer
    {
        public void Init(IPlayerBehavior player);
        public void ListenMousePos(Vector3 mousePos);
    }
}