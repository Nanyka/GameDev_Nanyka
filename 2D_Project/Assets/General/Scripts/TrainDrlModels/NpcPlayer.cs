using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
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
        [SerializeField] private VoidChannel resetGameChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private IndexAndPlotTranslator indexTranslator;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private ListIntStorage gameBoard;
        [SerializeField] private GameObject playerController;

        private Agent _agent;
        private IPlayerBehavior _playerBehavior;
        private float wrongPlotPunishment = -0.5f;
        private float suviveReward = 0.1f;
        private float winReward = 10f;

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
                _agent.AddReward(wrongPlotPunishment);
                endGameChannel.ExecuteChannel(false); // End episode when the agent fail to select a plot
            }
            else
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(action));
        }
        
        private void OnPlayATurn()
        {
            if (_playerBehavior.GetPlayerId() == currentPlayer.GetValue())
            {
                AskForAction();
                _agent.AddReward(suviveReward);
            }
        }
        
        private void OnEpisodeEnd(bool hasWinner)
        {
            // Grant a reward for this agent if it is the winner
            if (hasWinner && _playerBehavior.GetPlayerId() == currentPlayer.GetValue())
                _agent.AddReward(winReward);
            
            _agent.EndEpisode();
            
            // Assign current player to reset game
            if (_playerBehavior.GetPlayerId() == currentPlayer.GetValue())
                resetGameChannel.ExecuteChannel();
        }
        
        #region AGENT INTERACTION

        private void AskForAction()
        {
            if (_playerBehavior.GetPlayerId() != currentPlayer.GetValue())
                return;

            if (_agent == null)
                return;

            if (Academy.Instance.IsCommunicatorOn)
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

            if (Input.GetKeyDown(KeyCode.S))
            {
                currentAction = 1;
                // AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentAction = 2;
                // AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                currentAction = 3;
                // AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentAction = 4;
                // AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                currentAction = 5;
                // AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                currentAction = 6;
                // AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                currentAction = 7;
                // AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                currentAction = 8;
                // AskForAction();
            }
        }
        
        #endregion
    }
}