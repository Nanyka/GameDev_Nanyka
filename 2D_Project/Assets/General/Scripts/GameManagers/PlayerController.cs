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
        // [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private TwoIntChannel announceStateChanged;
        // [SerializeField] private VoidChannel countCircles;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private int playerId;

        private IObjectPool _objectPool;
        private List<ICircleTrait> _circles = new();

        private (int, int, int)[] winningCombinations = new[]
        {
            (0, 1, 2), (3, 4, 5), (6, 7, 8), // Horizontal
            (0, 3, 6), (1, 4, 7), (2, 5, 8), // Vertical
            (0, 4, 8), (2, 4, 6) // Diagonal
        };

        private bool _isPlayed;

        private void Awake()
        {
            m_PlayerBehavior.SetValue(this);
            _objectPool = GetComponent<IObjectPool>();
        }

        private void OnEnable()
        {
            mousePosChannel.AddListener(InTurnPlay);
            changePlayerChannel.AddListener(OnChangePlayer);
            resetGameChannel.AddListener(ResetPlayer);
        }

        private void OnDisable()
        {
            mousePosChannel.RemoveListener(InTurnPlay);
            changePlayerChannel.AddListener(OnChangePlayer);
            resetGameChannel.RemoveListener(ResetPlayer);
        }

        private void OnChangePlayer()
        {
            _isPlayed = false;
        }

        #region INTERFACE FUNCTIONS

        public void InTurnPlay(Vector3 clickPoint)
        {
            if (_isPlayed)
            {
                Debug.Log("You played this turn already");
                return;
            }

            if (currentPlayer.GetValue() == playerId && CheckCircleExist(clickPoint) == false)
            {
                var spawnObject = _objectPool.GetObject();
                if (spawnObject.TryGetComponent(out ICircleTrait circle))
                {
                    _circles.Add(circle);
                    spawnObject.SetActive(true);
                    circle.Init(clickPoint);
                    _isPlayed = true;
                    enableEndButtonChannel.ExecuteChannel();
                    announceStateChanged.ExecuteChannel(playerId,circle.GetId());
                }

                // CheckWin();
            }
        }

        public void ResetPlayer()
        {
            _objectPool.ResetPool();
            _circles.Clear();
            _isPlayed = false;
        }

        public int GetPlayerId()
        {
            return playerId;
        }

        #endregion

        private bool CheckCircleExist(Vector3 clickPoint)
        {
            bool isCircleExisted = false;

            foreach (var checkedCircle in _circles)
            {
                if (checkedCircle.DetectTouchPoint(clickPoint))
                {
                    isCircleExisted = true;
                    break;
                }
            }

            return isCircleExisted;
        }

        // private void CheckWin()
        // {
        //     bool isWin = false;
        //     var positionList = _circles.Select(t => t.GetId()).ToList();
        //     foreach (var combination in winningCombinations)
        //     {
        //         if (positionList.Contains(combination.Item1)
        //             && positionList.Contains(combination.Item2)
        //             && positionList.Contains(combination.Item3))
        //         {
        //             isWin = true;
        //             break;
        //         }
        //     }
        //
        //     if (isWin)
        //         endGameChannel.ExecuteChannel(true);
        //     else
        //         countCircles.ExecuteChannel();
        // }
    }
}