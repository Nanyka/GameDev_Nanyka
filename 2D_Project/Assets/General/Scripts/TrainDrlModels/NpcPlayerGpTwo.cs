using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine;

namespace TheAiAlchemist
{
    public class NpcPlayerGpTwo : MonoBehaviour, INpcPlayer
    {
        public int currentAction = -1;
        public int currentPriority = 0;

        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private VoidChannel newGameChannel;
        [SerializeField] private VoidChannel interruptGameChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private IntChannel combatChannel;
        [SerializeField] private IndexAndPlotTranslator indexTranslator;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private ListCircleStorage gameBoard;
        [SerializeField] private GameObject playerController;

        private Agent _agent;
        private BehaviorType behaviorType;
        private IPlayerBehavior _playerBehavior;
        private IInventoryComp _inventoryComp;
        private float combatReward = 0.1f;
        private float winReward = 1f;

        public void Awake()
        {
            // Since this example does not inherit from the Agent class, explicit registration
            // of the RpcCommunicator is required. The RPCCommunicator should only be compiled
            // for Standalone platforms (i.e. Windows, Linux, or Mac)
#if UNITY_EDITOR || UNITY_STANDALONE
            if (!CommunicatorFactory.CommunicatorRegistered)
            {
                CommunicatorFactory.Register<ICommunicator>(RpcCommunicator.Create);
            }
#endif
        }

        private void OnEnable()
        {
            _agent = GetComponent<Agent>();
            behaviorType = GetComponent<BehaviorParameters>().BehaviorType;
            _playerBehavior = playerController.GetComponent<IPlayerBehavior>();
            _inventoryComp = GetComponent<IInventoryComp>();
            _inventoryComp.ResetInventory();

            endGameChannel.AddListener(OnEpisodeEnd);
            newGameChannel.AddListener(OnPlayATurn);
            changePlayerChannel.AddListener(OnPlayATurn);
            combatChannel.AddListener(InACombat);
        }

        private void OnDisable()
        {
            endGameChannel.RemoveListener(OnEpisodeEnd);
            newGameChannel.RemoveListener(OnPlayATurn);
            changePlayerChannel.RemoveListener(OnPlayATurn);
            combatChannel.RemoveListener(InACombat);
        }

        #region INTERFACE FUNCTIONS

        public void TakeAction(ActionSegment<int> action)
        {
            // Check if the action[0] is available: not at the same playerId
            // Check the action[1] is at higher priority than the opponent
            // Remember to add 1 into action[1] to turn from zero-based space to one-based space
            var plotValue = gameBoard.GetValue()[action[0]];

            if (_inventoryComp.IsProductAvailable(action[1]) == false)
            {
                // Debug.Log($"Run out of circle: {action[1] + 1}");
                interruptGameChannel.ExecuteChannel();
            }
            else if (plotValue == null)
            {
                _inventoryComp.Withdraw(action[1]);
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(action[0]), action[1] + 1);
            }
            else if (plotValue.GetPlayerId() == _playerBehavior.GetPlayerId())
            {
                interruptGameChannel.ExecuteChannel();
                // Debug.Log("Same player");
            }
            else if ((action[1] + 1) <= plotValue.GetPriority())
            {
                interruptGameChannel.ExecuteChannel();
                // Debug.Log($"Unavailable priority. Current: {plotValue.GetPriority()}, New: {action[1]+1}");
            }
            else
            {
                _inventoryComp.Withdraw(action[1]);
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(action[0]), action[1] + 1);
                // Add score for this one
                combatChannel.ExecuteChannel(_playerBehavior.GetPlayerId());
                
                // Debug.Log("Higher priority");
            }
        }

        public int GetCurrentAction()
        {
            return currentAction;
        }

        public int GetCurrentPriority()
        {
            return currentPriority;
        }

        #endregion

        private void OnPlayATurn()
        {
            if (_playerBehavior.GetPlayerId() == currentPlayer.GetValue())
                AskForAction();
        }

        private void OnEpisodeEnd(bool hasWinner)
        {
            var currentMultiplier = _playerBehavior.GetPlayerId() == currentPlayer.GetValue() ? 1 : -1;
            _agent.AddReward(hasWinner ? winReward * currentMultiplier : 0f);
            _agent.EndEpisode();

            // Fill up inventory
            _inventoryComp.ResetInventory();
        }

        #region AGENT INTERACTION

        private void AskForAction()
        {
            if (_playerBehavior.GetPlayerId() != currentPlayer.GetValue())
                return;

            if (_agent == null)
                return;

            if (Academy.Instance.IsCommunicatorOn || behaviorType == BehaviorType.InferenceOnly)
                _agent?.RequestDecision();
            else
                StartCoroutine(WaitToRequestDecision());
        }

        private IEnumerator WaitToRequestDecision()
        {
            currentAction = -1;
            yield return new WaitUntil(() => currentAction >= 0);
            _agent?.RequestDecision();
        }
        
        private void InACombat(int attacker)
        {
            _agent.AddReward(attacker == _playerBehavior.GetPlayerId()? combatReward: -combatReward);
        }

        #endregion

        #region FOR TESTING AGENT

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                currentAction = 0;
            else if (Input.GetKeyDown(KeyCode.X))
                currentAction = 1;
            else if (Input.GetKeyDown(KeyCode.C))
                currentAction = 2;
            else if (Input.GetKeyDown(KeyCode.A))
                currentAction = 3;
            else if (Input.GetKeyDown(KeyCode.S))
                currentAction = 4;
            else if (Input.GetKeyDown(KeyCode.D))
                currentAction = 5;
            else if (Input.GetKeyDown(KeyCode.Q))
                currentAction = 6;
            else if (Input.GetKeyDown(KeyCode.W))
                currentAction = 7;
            else if (Input.GetKeyDown(KeyCode.E))
                currentAction = 8;

            else if (Input.GetKeyDown(KeyCode.I))
                currentPriority = 0;
            else if (Input.GetKeyDown(KeyCode.O))
                currentPriority = 1;
            else if (Input.GetKeyDown(KeyCode.P))
                currentPriority = 2;
        }

        #endregion
    }
}