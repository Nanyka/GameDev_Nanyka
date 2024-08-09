using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        [SerializeField] private VoidChannel endGameChannel;
        [SerializeField] private VoidChannel resetGameChannel;
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
                }

                CheckWin();
            }
        }

        public void ResetPlayer()
        {
            _objectPool.ResetPool();
            _circles.Clear();
            _isPlayed = false;
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

        private void CheckWin()
        {
            bool isWin = false;
            var positionList = _circles.Select(t => t.GetId()).ToList();
            foreach (var combination in winningCombinations)
            {
                if (positionList.Contains(combination.Item1)
                    && positionList.Contains(combination.Item2)
                    && positionList.Contains(combination.Item3))
                {
                    isWin = true;
                    break;
                }
            }

            if (isWin)
                endGameChannel.ExecuteChannel();

            // var combinations = _circles.DifferentCombinations(3);
            // foreach (var combination in combinations)
            // {
            //     var score = combination.Sum(t => t.GetId());
            //     var includeFour = combination.Any(t => t.GetId() == 4);
            //     bool isWin = false;
            //
            //     if (includeFour)
            //         isWin = score == 12;
            //     else if ((score - 3) % 6 == 0)
            //     {
            //         var orderedCom = combination.OrderByDescending(t => t.GetId());
            //         var pairOne = orderedCom.ElementAt(0).GetId() - orderedCom.ElementAt(1).GetId();
            //         var pairTwo = orderedCom.ElementAt(1).GetId() - orderedCom.ElementAt(2).GetId();
            //         isWin = pairOne == pairTwo;
            //     }
            //
            //     if (isWin)
            //         endGameChannel.ExecuteChannel();
            // }
        }
    }
}