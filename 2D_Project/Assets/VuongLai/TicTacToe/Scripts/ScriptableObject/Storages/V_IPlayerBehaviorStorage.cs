using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_IPlayerBehaviorStorage", menuName = "ScriptableObject/Storage/V_IPlayerBehaviorStorage")]
    public class V_IPlayerBehaviorStorage : ScriptableObject
    {
        private IPlayerBehavior value;

        public void SetValue(IPlayerBehavior value)
        {
            this.value = value;
        }

        public IPlayerBehavior GetValue()
        {
            return value;
        }
    }
}
