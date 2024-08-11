using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "PlayerInfor", menuName = "ScriptableObject/PlayerInfor")]
    public class PlayerInfor : ScriptableObject
    {
        private bool isFirstPlayer = true;
        private IPlayerBehavior currentPlayerBehavior;

        public void ChangePlayer()
        {
            isFirstPlayer = !isFirstPlayer;
        }

        public void SetIsFirstPlayer()
        {
            isFirstPlayer = true;
        }

        public bool IsFirstPlayer()
        {
            return isFirstPlayer;
        }

        public void SetCurrentPlayerBehavior(IPlayerBehavior playerBehavior)
        {
            currentPlayerBehavior = playerBehavior;
        }

        public void RunEventCurrentPlayerTalk()
        {
            if (currentPlayerBehavior != null)
                currentPlayerBehavior.PlayerTalk();
        }
    }
}