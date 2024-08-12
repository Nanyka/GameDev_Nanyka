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
    public class NpcPlayer : MonoBehaviour
    {
        public int currentAction = -1;

        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private VoidChannel newGameChannel;
        [SerializeField] private VoidChannel interruptGameChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private IndexAndPlotTranslator indexTranslator;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private ListIntStorage gameBoard;
        [SerializeField] private GameObject playerController;

        private Agent _agent;
        private BehaviorType behaviorType;

        private IPlayerBehavior _playerBehavior;
        // private int turnCount;

        // private float wrongPlotPunishment = -0.5f;
        private float surviveReward = 0.1f;
        private float winReward = 1f;
        private int maxTurnCount = 10;

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

        public void TakeAction(int action)
        {
            // Send the index and playerId to GameStateManager
            if (gameBoard.GetValue()[action] != 0)
            {
                // Debug.Log($"Player {_playerBehavior.GetPlayerId()} place on an unavailable plot");
                interruptGameChannel.ExecuteChannel(); // v1: End episode when the agent go beyond maximum step
                // OnPlayATurn(); // v2: End episode when the agent go beyond maximum step
            }
            else
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(action));
        }

        private void OnPlayATurn()
        {
            // var currentMultiplier = _playerBehavior.GetPlayerId() == currentPlayer.GetValue() ? 1 : -1;
            // currentMultiplier *= turnCount;
            // _agent.AddReward(surviveReward * currentMultiplier); // v1 --> v4

            if (_playerBehavior.GetPlayerId() == currentPlayer.GetValue())
            {
                // v1: End episode when the agent go beyond maximum step
                AskForAction();

                // v2: End episode when the agent go beyond maximum step
                // if (turnCount >= maxTurnCount)
                //     interruptGameChannel.ExecuteChannel();
                // else
                //     AskForAction();

                // v5: Add reward any playing turn
                // _agent.AddReward(surviveReward);
            }

            // turnCount++;
            // Debug.Log($"Player {_playerBehavior.GetPlayerId()} reward: {_agent.GetCumulativeReward()}");
        }

        private void OnEpisodeEnd(bool hasWinner)
        {
            // Grant a reward for this agent if it is the winner and adverse for the loser
            // If the agent go wrong plot, punish it a small reward and grant this reward to another
            var currentMultiplier = _playerBehavior.GetPlayerId() == currentPlayer.GetValue() ? 1 : -1;
            // _agent.AddReward(hasWinner ? winReward * currentMultiplier : wrongPlotPunishment * currentMultiplier); // v1 --> v4
            _agent.AddReward(hasWinner ? winReward * currentMultiplier : 0f); // v5

            // Debug.Log($"Player {_playerBehavior.GetPlayerId()} final reward: {_agent.GetCumulativeReward()}");
            _agent.EndEpisode();
            // turnCount = 0;

            // Assign current player to reset game
            // if (_playerBehavior.GetPlayerId() != currentPlayer.GetValue())
            //     resetGameChannel.ExecuteChannel();
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

        #endregion

        #region FOR TESTING AGENT

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentAction = 0;
                // AskForAction();
            }

            else if (Input.GetKeyDown(KeyCode.S))
            {
                currentAction = 1;
                // AskForAction();
            }

            else if (Input.GetKeyDown(KeyCode.D))
            {
                currentAction = 2;
                // AskForAction();
            }

            else if (Input.GetKeyDown(KeyCode.F))
            {
                currentAction = 3;
                // AskForAction();
            }

            else if (Input.GetKeyDown(KeyCode.Z))
            {
                currentAction = 4;
                // AskForAction();
            }

            else if (Input.GetKeyDown(KeyCode.X))
            {
                currentAction = 5;
                // AskForAction();
            }

            else if (Input.GetKeyDown(KeyCode.C))
            {
                currentAction = 6;
                // AskForAction();
            }

            else if (Input.GetKeyDown(KeyCode.V))
            {
                currentAction = 7;
                // AskForAction();
            }

            else if (Input.GetKeyDown(KeyCode.B))
            {
                currentAction = 8;
                // AskForAction();
            }
        }

        public int GetPlayerId()
        {
            return _playerBehavior.GetPlayerId();
        }

        #endregion
    }
}