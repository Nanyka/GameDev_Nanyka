using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TicTacToeAgentGpTwo : Agent
    {
        [SerializeField] protected ListCircleStorage gameBoard;
        [SerializeField] private GameObject playerController;

        protected INpcPlayer _controller;
        // protected IInventoryComp _inventory;
        protected IPlayerBehavior _playerBehavior;

        public override void Initialize()
        {
            _controller = GetComponent<INpcPlayer>();
            _playerBehavior = playerController.GetComponent<IPlayerBehavior>();
        }

        // Collect current player state as an array of existing circles in conjunction with unoccupied slots
        public override void CollectObservations(VectorSensor sensor)
        {
            // string observation = "";
            foreach (var plot in gameBoard.GetValue())
            {
                int addPlayerId = 0;
                int addPriority = 0;
                if (plot != null)
                {
                    addPlayerId = plot.GetPlayerId();
                    addPriority = plot.GetPriority();
                    // observation += $"{plot.GetId()}.({addPlayerId},{addPriority}),";
                }

                sensor.AddOneHotObservation(addPlayerId, 3); // 3x9 =27
                sensor.AddOneHotObservation(addPriority,3); // 3x9 = 27
            }
            // Debug.Log(observation);

            // Collect inventory state
            // _inventory = _playerBehavior.GetInventory();
            var inventory = _playerBehavior.GetInventory().GetItems();
            foreach (var item in inventory)
                sensor.AddObservation(item); // 3x1 = 3
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            // Debug.Log($"Player action: {actions.DiscreteActions[0]}, {actions.DiscreteActions[1]}");
            _controller.TakeAction(actions.DiscreteActions);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionOut = actionsOut.DiscreteActions;
            discreteActionOut[0] = _controller.GetCurrentAction();
            discreteActionOut[1] = _controller.GetCurrentPriority();
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            for (int i = 0; i < gameBoard.GetValue().Count; i++)
            {
                if (gameBoard.GetValue()[i] != null)
                {
                    actionMask.SetActionEnabled(0, i,
                        gameBoard.GetValue()[i].GetPlayerId() != _playerBehavior.GetPlayerId());
                    // actionDisable += $"{i},";
                }
            }
            // Debug.Log(actionDisable);
        }
    }
}