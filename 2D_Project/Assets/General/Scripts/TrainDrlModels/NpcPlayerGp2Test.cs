using UnityEngine;

namespace TheAiAlchemist
{
    public class NpcPlayerGp2Test : NpcPlayerGp2v2
    {
        [SerializeField] private NpcChannel WaitForAction;

        protected override void OnPlayATurn()
        {
            WaitForAction.ExecuteChannel(this);
        }
    }
}