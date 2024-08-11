using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class SecondPlayerController : MonoBehaviour, IPlayerBehavior
    {
        [SerializeField] private V_IPlayerBehaviorStorage m_PlayerBehavior;

        private void Awake()
        {
            m_PlayerBehavior.SetValue(this);
        }

        public void PlayerTalk()
        {
            Debug.Log("I am SecondPlayer");
        }
    }
}