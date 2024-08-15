using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

// Spawn circles if no any circle is existing at the click point
// Change state of the circle if it is enable before
// Decide win condition
// Reset player

namespace TheAiAlchemist
{
    public class PlayerController : MonoBehaviour, IPlayerBehavior
    {
        [SerializeField] private IPlayerBehaviorStorage m_PlayerBehavior;
        [SerializeField] private Vector3Channel mousePosChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private VoidChannel enableEndButtonChannel;
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private CircleChannel announceStateChanged;
        [SerializeField] private CircleChannel disableCircleChannel;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private int playerId;

        private IObjectPool _objectPool;
        private List<ICircleTrait> _circles = new();
        private bool _isPlayed;

        private void Awake()
        {
            m_PlayerBehavior.SetValue(this);
            _objectPool = GetComponent<IObjectPool>();
        }

        private void OnEnable()
        {
            mousePosChannel.AddListener(ListenMousePos);
            changePlayerChannel.AddListener(OnChangePlayer);
            resetGameChannel.AddListener(ResetPlayer);
            disableCircleChannel.AddListener(DisableCircle);
        }

        private void OnDisable()
        {
            mousePosChannel.RemoveListener(ListenMousePos);
            changePlayerChannel.RemoveListener(OnChangePlayer);
            resetGameChannel.RemoveListener(ResetPlayer);
            disableCircleChannel.RemoveListener(DisableCircle);
        }

        private void OnChangePlayer()
        {
            _isPlayed = false;
        }

        #region INTERFACE FUNCTIONS

        private void ListenMousePos(Vector3 mousePos)
        {
            if (_isPlayed)
            {
                Debug.Log("You played this turn already");
                return;
            }

            if (currentPlayer.GetValue() == playerId)
                InTurnPlay(mousePos, 0);
        }

        public void InTurnPlay(Vector3 clickPoint, int priority)
        {
            var spawnObject = _objectPool.GetObject();
            if (spawnObject.TryGetComponent(out ICircleTrait circle))
            {
                _circles.Add(circle);
                spawnObject.SetActive(true);
                circle.Init(clickPoint, playerId, priority);
                _isPlayed = true;
                // Debug.Log($"Player {playerId} places at {circle.GetId()}");
                enableEndButtonChannel.ExecuteChannel();
                announceStateChanged.ExecuteChannel(circle);

                // Disable the opponent's circle if possible
                disableCircleChannel.ExecuteChannel(circle);
            }
        }

        private void ResetPlayer()
        {
            _objectPool.ResetPool();
            _circles.Clear();
            _isPlayed = false;
        }

        public int GetPlayerId()
        {
            return playerId;
        }

        public void DisableCircle(ICircleTrait circle)
        {
            if (circle.GetPlayerId() == playerId)
                return;
            
            var disableCircle = _circles.Find(t => t.GetId() == circle.GetId());
            disableCircle?.DisableCircle();
        }

        #endregion

        // private bool CheckCircleExist(Vector3 clickPoint)
        // {
        //     bool isCircleExisted = false;
        //
        //     foreach (var checkedCircle in _circles)
        //     {
        //         if (checkedCircle.DetectTouchPoint(clickPoint))
        //         {
        //             isCircleExisted = true;
        //             break;
        //         }
        //     }
        //
        //     return isCircleExisted;
        // }
    }
}