using UnityEngine;

namespace TheAiAlchemist
{
    public class StateComp : MonoBehaviour,ICheckState
    {
        private bool isActivated = true;
        
        public bool IsActivated()
        {
            return isActivated;
        }

        public void ChangeState()
        {
            isActivated = !isActivated;
        }
    }
}