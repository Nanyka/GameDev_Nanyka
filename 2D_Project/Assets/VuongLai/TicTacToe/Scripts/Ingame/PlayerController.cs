using System.Collections;
using System.Collections.Generic;
using TheAiAlchemist;
using UnityEngine;

namespace V_TicTacToe
{
    public class PlayerController : MonoBehaviour, IPlayerBehavior
    {
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_BooleanStorage isPlayed;
        [SerializeField] private V_Vector3Channel touchItemChannel;
        [SerializeField] private V_VoidChannel endTurnChannel;
        [SerializeField] private int playerId;

        private ObjectPool _objectPool;

        private void Awake()
        {
            _objectPool = GetComponent<ObjectPool>();
            isPlayed.SetValue(false);
        }

        private void OnEnable()
        {
            touchItemChannel.AddListener(TouchItem);
        }

        private void OnDisable()
        {
            touchItemChannel.RemoveListener(TouchItem);
        }

        public void PlayerTalk()
        {
            Debug.Log($"Is Turn Player have playerId {currentPlayerId.GetValue()}");
        }

        private void TouchItem(Vector3 touchPosition)
        {
            if (isPlayed.GetValue())
            {
                return;
            }

            if (currentPlayerId.GetValue().Equals(playerId))
            {
                isPlayed.SetValue(true);

                GameObject itemObject = _objectPool.GetObject();

                itemObject.SetActive(true);

                ICheckItemStatus checkItem = itemObject.GetComponent<ICheckItemStatus>();
                if (checkItem != null)
                {
                    checkItem.Init(touchPosition);
                    checkItem.SetShowItem(true);
                }

                endTurnChannel.RunVoidChannel();
            }
        }

        private void EndTurn()
        {
            isPlayed.SetValue(false);
        }
    }
}

