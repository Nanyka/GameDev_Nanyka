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
        [SerializeField] private int playerId;

        [Header("Channel")]
        [SerializeField] private V_Vector2Channel checkWinChannel;
        [SerializeField] private V_VoidChannel resetLevelChannel;

        [Header("Storage")]
        [SerializeField] private V_Vector2Storage currentMatrixPosition;

        private ObjectPool _objectPool;

        private void Awake()
        {
            _objectPool = GetComponent<ObjectPool>();
            isPlayed.SetValue(false);
        }

        private void OnEnable()
        {
            touchItemChannel.AddListener(TouchItem);
            resetLevelChannel.AddListener(Reset);
        }

        private void OnDisable()
        {
            touchItemChannel.RemoveListener(TouchItem);
            resetLevelChannel.RemoveListener(Reset);
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

                ICheckItemStatus checkItem = itemObject.GetComponent<ICheckItemStatus>();
                if (checkItem != null)
                {
                    checkItem.Init(touchPosition);
                    checkItem.SetShowItem(true);

                    itemObject.SetActive(true);

                    checkWinChannel.RunVector2Channel(currentMatrixPosition.Value);
                }
            }
        }

        private void Reset()
        {
            if (_objectPool != null)
            {
                _objectPool.ResetPool();
            }
        }
    }
}

