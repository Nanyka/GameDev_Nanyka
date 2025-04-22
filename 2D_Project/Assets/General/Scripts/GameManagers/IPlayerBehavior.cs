using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheAiAlchemist
{
    public interface IPlayerBehavior
    {
        public bool IsPlayed { get; set; }
        public void InTurnPlay(Vector3 clickPoint, int priority);
        public int GetPlayerId();
        public void DisableCircle(ICircleTrait circle);
        public IInventoryComp GetInventory();
    }
}
