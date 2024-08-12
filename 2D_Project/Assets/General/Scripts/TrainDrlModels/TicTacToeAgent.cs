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

        private IPlayerBehavior _playerBehavior;
        
        public override void Initialize()
        {
            _playerBehavior = playerController.GetComponent<IPlayerBehavior>();
        }

        // Collect current player state as an array of existing circles in conjunction with unoccupied slots
        public override void CollectObservations(VectorSensor sensor)
        {
            // string observation = "";
            foreach (var plot in gameBoard.GetValue())
            {
                if (plot == 0)
                {
                    sensor.AddObservation(0);
                    // observation += $"0,";
                }
                else
                {
                    int addValue = plot == _playerBehavior.GetPlayerId() ? 1 : -1;
                    sensor.AddObservation(addValue);
                    // observation += $"{addValue},";
                }
            }

            // Debug.Log(observation);
        }

        public override void OnEpisodeBegin()
        {
            // Debug.Log("Episode begin");
        }
    }
}
