using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Spawn circles if no any circle is existing at the click point
// Change state of the circle if it is enable before
// Decide win condition

namespace TheAiAlchemist
{
    public class PlayerController : MonoBehaviour, IPlayerBehavior
    {
        [SerializeField] private IPlayerBehaviorStorage m_PlayerBehavior;
        [SerializeField] private Vector3Channel mousePosChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private VoidChannel enableEndButtonChannel;
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
            mousePosChannel.AddListener(InTurnPlay);
            changePlayerChannel.AddListener(OnChangePlayer);
        }

        private void OnDisable()
        {
            mousePosChannel.RemoveListener(InTurnPlay);
            changePlayerChannel.AddListener(OnChangePlayer);
        }

        private void OnChangePlayer()
        {
            _isPlayed = false;

            // var results = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.DifferentCombinations(3);
            // foreach (var combination in results)
            // {
            //     string printCombination = "";
            //     foreach (var item in combination)
            //     {
            //         printCombination += $"{item},";
            //     }
            //     printCombination += "\n";
            //     Debug.Log(printCombination);
            // }
        }

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
                    // spawnObject.transform.position = clickPoint;
                    spawnObject.SetActive(true);
                    circle.Init(clickPoint);
                    _isPlayed = true;
                    enableEndButtonChannel.ExecuteChannel();
                }
                
                CheckWin();
            }
        }

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
            var combinations = _circles.DifferentCombinations(3);
            foreach (var combination in combinations)
            {
                // string combinationString = "";
                // foreach (var item in combination)
                //     combinationString += $"{item.GetId()} ";
                // Debug.Log(combination.Any(t => t.GetId()==4));
                
                var score = combination.Sum(t => t.GetId());
                var includeFour = combination.Any(t => t.GetId() == 4);
                bool isWin = false;

                if (includeFour)
                    isWin = score == 12;
                else if ((score - 3) % 6 == 0)
                {
                    var orderedCom = combination.OrderByDescending(t => t.GetId());
                    var pairOne = orderedCom.ElementAt(0).GetId() - orderedCom.ElementAt(1).GetId();
                    var pairTwo = orderedCom.ElementAt(1).GetId() - orderedCom.ElementAt(2).GetId();
                    isWin = pairOne == pairTwo;
                }

                if (isWin)
                    Debug.Log($"Player {playerId} win!");

                // if (includeFour && score == 12)
                // {
                //     Debug.Log($"Player {playerId} win with the combination: {combinationString}. Total: {combination.Sum(t => t.GetId())}");
                // }
                // else if ((score - 3) % 6 == 0)
                // {
                //     Debug.Log($"Player {playerId} win with the combination: {combinationString}. Total: {combination.Sum(t => t.GetId())}");
                // }
            }
        }
    }
}