using System;
using UnityEngine;

namespace TheAiAlchemist
{
    public class PlayerUnitPlacer: MonoBehaviour, IUnitPlacer
    {
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private IntStorage askUnitIndex;
        [SerializeField] private Vector3Channel mousePosChannel;

        private IPlayerBehavior playerBehavior;

        private void OnEnable()
        {
            mousePosChannel.AddListener(ListenMousePos);
        }

        private void OnDisable()
        {
            mousePosChannel.RemoveListener(ListenMousePos);
        }

        public void Init(IPlayerBehavior player)
        {
            playerBehavior = player;
        }
        
        public void ListenMousePos(Vector3 mousePos)
        {
            if (playerBehavior.IsPlayed)
            {
                Debug.Log("You played this turn already");
                return;
            }

            if (currentPlayer.GetValue() == playerBehavior.GetPlayerId())
            {
                if (askUnitIndex.GetValue() < 0)
                {
                    Debug.Log("Need to select a unit");
                }
                else
                {
                    playerBehavior.GetInventory().Withdraw(askUnitIndex.GetValue());
                    playerBehavior.InTurnPlay(mousePos, askUnitIndex.GetValue());
                }
            }
        }
    }
}