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

        [SerializeField] private IndexAndPlotTranslator indexTranslator;
        [SerializeField] private IntStorage currentPlayer;
        [SerializeField] private ListIntStorage gameBoard;
        [SerializeField] private GameObject playerController;

        private Agent _agent;
        private IPlayerBehavior _playerBehavior;

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

        private void Start()
        {
            _agent = GetComponent<Agent>();
            _playerBehavior = playerController.GetComponent<IPlayerBehavior>();
        }

        public void TakeAction(int action)
        {
            // Debug.Log($"Current action is : {action}");

            // Send the index and playerId to GameStateManager
            if (gameBoard.GetValue()[action] != 0)
                Debug.Log($"Player {_playerBehavior.GetPlayerId()} place on an unavailable plot");
            else
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(action));
        }

        #region FOR TESTING AGENT

        private void Update()
        {
            // TODO: get the key down
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentAction = 0;
                AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                currentAction = 1;
                AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                currentAction = 2;
                AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                currentAction = 3;
                AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                currentAction = 4;
                AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                currentAction = 5;
                AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                currentAction = 6;
                AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                currentAction = 7;
                AskForAction();
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
                currentAction = 8;
                AskForAction();
            }
        }

        public void AskForAction()
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
            yield return new WaitUntil(() => Input.anyKeyDown);
            _agent?.RequestDecision();
        }

        #endregion
    }
}