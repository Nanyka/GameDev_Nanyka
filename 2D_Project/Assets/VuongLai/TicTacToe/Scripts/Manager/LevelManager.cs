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
        [SerializeField] private V_VoidChannel resetLevelChannel;

        private void Awake()
        {
            isFirstPlayer.SetValue(true);

            StartCoroutine(CRStartGame());
        }

        private IEnumerator CRStartGame()
        {
            yield return null;

            StartGame();
        }

        private void OnEnable()
        {
            changePlayerChannel.AddListener(ChangePlayer);
            touchItemChannel.AddListener(OnTouchItem);
            resetLevelChannel.AddListener(OnResetLevel);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(ChangePlayer);
            touchItemChannel.RemoveListener(OnTouchItem);
            resetLevelChannel.RemoveListener(OnResetLevel);
        }

        private void StartGame()
        {
            currentPlayerId.Value = 0;
            showIngameMenuChannel.RunVoidChannel();
        }

        private void OnResetLevel()
        {
            currentPlayerId.Value = 0;
            showIngameMenuChannel.RunVoidChannel();
        }

        public void ChangePlayer()
        {
            if (currentPlayerId.Value.Equals(0))
            {
                Debug.Log("Turn of Player1");
            }
            else if (currentPlayerId.Value.Equals(1))
            {
                Debug.Log("Turn of Player2");
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