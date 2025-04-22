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
        // public bool IsPlayed { get; private set; }

        [SerializeField] private IPlayerBehaviorStorage m_PlayerBehavior;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private VoidChannel enableEndButtonChannel;
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private CircleChannel announceStateChanged;
        [SerializeField] private CircleChannel disableCircleChannel;
        [SerializeField] private ListCircleStorage gameBoard;
        [SerializeField] protected IndexAndPlotTranslator indexTranslator;
        [SerializeField] private int playerId;

        private IObjectPool _objectPool;
        private List<ICircleTrait> _circles = new();
        private IInventoryComp _inventoryComp;
        private IUnitPlacer _unitPlacer;

        private void Awake()
        {
            m_PlayerBehavior.SetValue(this);
            _objectPool = GetComponent<IObjectPool>();
            _inventoryComp = GetComponent<IInventoryComp>();
            _inventoryComp.ResetInventory();
            _unitPlacer = GetComponent<IUnitPlacer>();
            _unitPlacer.Init(this);
        }

        private void OnEnable()
        {
            changePlayerChannel.AddListener(OnChangePlayer);
            resetGameChannel.AddListener(ResetPlayer);
            disableCircleChannel.AddListener(DisableCircle);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(OnChangePlayer);
            resetGameChannel.RemoveListener(ResetPlayer);
            disableCircleChannel.RemoveListener(DisableCircle);
        }

        private void OnChangePlayer()
        {
            IsPlayed = false;
        }

        #region INTERFACE FUNCTIONS

        public bool IsPlayed { get; set; }

        public void InTurnPlay(Vector3 clickPoint, int priority)
        {
            var spawnObject = _objectPool.GetObject();
            if (spawnObject.TryGetComponent(out ICircleTrait circle))
            {
                _circles.Add(circle);
                spawnObject.SetActive(true);
                circle.Init(clickPoint, playerId, priority);
                IsPlayed = true;
                enableEndButtonChannel.ExecuteChannel();
                announceStateChanged.ExecuteChannel(circle);
                disableCircleChannel.ExecuteChannel(circle);
            }
        }

        private void ResetPlayer()
        {
            _objectPool.ResetPool();
            _circles.Clear();
            IsPlayed = false;
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

        public IInventoryComp GetInventory()
        {
            return _inventoryComp;
        }

        #endregion
    }
}