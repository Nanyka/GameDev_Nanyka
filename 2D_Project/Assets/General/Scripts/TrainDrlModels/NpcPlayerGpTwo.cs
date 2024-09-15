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
        [SerializeField] protected VoidChannel interruptGameChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] protected CombatChannel combatChannel;
        [SerializeField] protected IndexAndPlotTranslator indexTranslator;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] protected ListCircleStorage gameBoard;
        [SerializeField] private GameObject playerController;

        private Agent _agent;
        private BehaviorType behaviorType;
        protected IPlayerBehavior _playerBehavior;
        // private float combatReward = 0f;
        private float survivalReward = 0.1f;
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
            // _inventoryComp = _playerBehavior.GetInventory();
            // _inventoryComp.ResetInventory();

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

        public virtual void TakeAction(ActionSegment<int> action)
        {
            // Check if the action[0] is available: not at the same playerId
            // Check the action[1] is at higher priority than the opponent
            // Remember to add 1 into action[1] to turn from zero-based space to one-based space
            var plotValue = gameBoard.GetValue()[action[0]];

            if (_playerBehavior.GetInventory().IsProductAvailable(action[1]) == false)
            {
                // Debug.Log($"Run out of circle: {action[1] + 1}");
                interruptGameChannel.ExecuteChannel();
            }
            else if (plotValue == null)
            {
                _playerBehavior.GetInventory().Withdraw(action[1]);
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(action[0]), action[1]);
            }
            else if (plotValue.GetPlayerId() == _playerBehavior.GetPlayerId())
            {
                interruptGameChannel.ExecuteChannel();
                // Debug.Log("Same player");
            }
            else if ((action[1] + 1) <= plotValue.GetPriority())
            {
                // Minus score for this one
                combatChannel.ExecuteChannel(_playerBehavior.GetPlayerId(),false);
                interruptGameChannel.ExecuteChannel();

                // Debug.Log($"Unavailable priority. Current: {plotValue.GetPriority()}, New: {action[1]+1}");
            }
            else
            {
                // Add score for this one
                combatChannel.ExecuteChannel(_playerBehavior.GetPlayerId(),true);
                _playerBehavior.GetInventory().Withdraw(action[1]);
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(action[0]), action[1]);
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

        protected virtual void OnPlayATurn()
        {
            if (_playerBehavior.GetPlayerId() == currentPlayer.GetValue())
            {
                _agent.AddReward(survivalReward);
                AskForAction();
            }
        }

        private void OnEpisodeEnd(bool hasWinner)
        {
            var currentMultiplier = _playerBehavior.GetPlayerId() == currentPlayer.GetValue() ? 1 : -1;
            _agent.AddReward(hasWinner ? winReward * currentMultiplier : 0f);
            // Debug.Log($"Player {_playerBehavior.GetPlayerId()} reward: {_agent.GetCumulativeReward()}");
            _agent.EndEpisode();

            // Fill up inventory
            _playerBehavior.GetInventory().ResetInventory();
        }

        #region AGENT INTERACTION

        public void AskForAction()
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
        
        private void InACombat(int attacker, bool isAttack)
        {
            // var reward = isAttack ? combatReward : -combatReward;
            // _agent.AddReward(attacker == _playerBehavior.GetPlayerId()? reward: -reward);
        }

        #endregion

        #region FOR TESTING AGENT

        protected virtual void Update()
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