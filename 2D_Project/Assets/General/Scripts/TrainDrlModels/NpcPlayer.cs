using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace TheAiAlchemist
{
    public class NpcPlayer : MonoBehaviour, INpcPlayer
    {
        public int currentAction = -1;

        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private VoidChannel newGameChannel;
        [SerializeField] private VoidChannel interruptGameChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private IndexAndPlotTranslator indexTranslator;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private ListCircleStorage gameBoard;
        [SerializeField] private GameObject playerController;

        private Agent _agent;
        private BehaviorType behaviorType;
        private IPlayerBehavior _playerBehavior;
        // private float surviveReward = 0.1f;
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
            endGameChannel.AddListener(OnEpisodeEnd);
            newGameChannel.AddListener(OnPlayATurn);
            changePlayerChannel.AddListener(OnPlayATurn);
        }

        private void OnDisable()
        {
            endGameChannel.RemoveListener(OnEpisodeEnd);
            newGameChannel.RemoveListener(OnPlayATurn);
            changePlayerChannel.RemoveListener(OnPlayATurn);
        }

        public void TakeAction(ActionSegment<int> action)
        {
            // Send the index and playerId to GameStateManager
            if (gameBoard.GetValue()[action[0]] != null)
                interruptGameChannel.ExecuteChannel();
            else
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(action[0]),0);
        }

        private void OnPlayATurn()
        {
            if (_playerBehavior.GetPlayerId() == currentPlayer.GetValue())
                AskForAction();
        }

        private void OnEpisodeEnd(bool hasWinner)
        {
            var currentMultiplier = _playerBehavior.GetPlayerId() == currentPlayer.GetValue() ? 1 : -1;
            _agent.AddReward(hasWinner ? winReward * currentMultiplier : 0f); // v5
            _agent.EndEpisode();
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
        }

        public int GetPlayerId()
        {
            return _playerBehavior.GetPlayerId();
        }

        public int GetCurrentAction()
        {
            return currentAction;
        }

        public int GetCurrentPriority()
        {
            return 0;
        }

        #endregion
    }
}