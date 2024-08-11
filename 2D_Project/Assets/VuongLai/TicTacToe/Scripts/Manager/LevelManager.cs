using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private V_VoidChannel changePlayerChannel;
        [SerializeField] private V_VoidChannel touchItemChannel;
        [SerializeField] private V_BooleanStorage isFirstPlayer;
        [SerializeField] private V_IPlayerBehaviorStorage player1;
        [SerializeField] private V_IPlayerBehaviorStorage player2;
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_VoidChannel showIngameMenuChannel;

        private void Awake()
        {
            isFirstPlayer.SetValue(true);

            StartGame();
        }

        private void OnEnable()
        {
            changePlayerChannel.AddListener(ChangePlayer);
            touchItemChannel.AddListener(OnTouchItem);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(ChangePlayer);
            touchItemChannel.RemoveListener(OnTouchItem);
        }

        private void StartGame()
        {
            showIngameMenuChannel.RunVoidChannel();
            currentPlayerId.SetValue(0);
        }

        public void ChangePlayer()
        {
            if(currentPlayerId.GetValue().Equals(0))
            {
                Debug.Log("turn of Player1");
            }
            else if(currentPlayerId.GetValue().Equals(1))
            {
                Debug.Log("turn of Player2");
            }
        }

        private void OnTouchItem()
        {
            if (isFirstPlayer.GetValue() == true)
                player1.GetValue().PlayerTalk();
            else
                player2.GetValue().PlayerTalk();
        }
    }
}