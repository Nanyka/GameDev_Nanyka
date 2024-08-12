using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TicTacToeAgent : Agent
    {
        [SerializeField] private ListIntStorage gameBoard;
        [SerializeField] private GameObject playerController;
        
        private NpcPlayer controller;
        private IPlayerBehavior _playerBehavior;
        
        public override void Initialize()
        {
            controller = GetComponent<NpcPlayer>();
            _playerBehavior = playerController.GetComponent<IPlayerBehavior>();
        }

        // Collect current player state as an array of existing circles in conjunction with unoccupied slots
        public override void CollectObservations(VectorSensor sensor)
        {
            // string observation = "";
            foreach (var plot in gameBoard.GetValue())
            {
                int addValue = 0;
                if (plot != 0)
                {
                    addValue = plot == _playerBehavior.GetPlayerId() ? 1 : -1;
                    // observation += $"{addValue},";
                }
                sensor.AddOneHotObservation(addValue,3);
            }

            // Debug.Log(observation);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            // Debug.Log($"Player {controller.GetPlayerId()} action: {actions.DiscreteActions[0]}");
            controller.TakeAction(actions.DiscreteActions[0]);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionOut = actionsOut.DiscreteActions;
            discreteActionOut[0] = controller.currentAction;
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            // string actionDisable = "";
            for (int i = 0; i < gameBoard.GetValue().Count; i++)
            {
                if (gameBoard.GetValue()[i] != 0)
                {
                    actionMask.SetActionEnabled(0, i, false);
                    // actionDisable += $"{i},";
                }
            }
            // Debug.Log(actionDisable);
        }
    }
}
