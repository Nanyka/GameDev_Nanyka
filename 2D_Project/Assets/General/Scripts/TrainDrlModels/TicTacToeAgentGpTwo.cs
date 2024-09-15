using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TicTacToeAgentGpTwo : Agent
    {
        [SerializeField] protected ListCircleStorage gameBoard;
        [SerializeField] protected IPlayerInfoStorage playerInfoStorage;
        [SerializeField] private GameObject playerController;

        protected INpcPlayer Controller;
        // protected IPlayerBehavior PlayerBehavior;
        // private IPlayerBehavior _opponentBehavior;
        protected int PlayerId;

        public override void Initialize()
        {
            Controller = GetComponent<INpcPlayer>();
            PlayerId = playerController.GetComponent<IPlayerBehavior>().GetPlayerId();
            // var playerId = playerController.GetComponent<IPlayerBehavior>().GetPlayerId();
            // PlayerBehavior = playerInfoStorage.GetValue().GetPlayerInfo(playerId).GetValue();
            // _opponentBehavior = playerInfoStorage.GetValue().GetOpponentInfo(playerId).GetValue();
            // _playerBehavior = playerController.GetComponent<IPlayerBehavior>();
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
            var playerInventory = playerInfoStorage.GetValue().GetPlayerInfo(PlayerId).GetValue().GetInventory().GetItems();
            var opponentInventory = playerInfoStorage.GetValue().GetOpponentInfo(PlayerId).GetValue().GetInventory().GetItems();
            foreach (var item in playerInventory)
                sensor.AddObservation(item); // 3x1 = 3
            
            foreach (var item in opponentInventory)
                sensor.AddObservation(item); // 3x1 = 3
        }
        
        public override void OnActionReceived(ActionBuffers actions)
        {
            // Debug.Log($"Player action: {actions.DiscreteActions[0]}, {actions.DiscreteActions[1]}");
            Controller.TakeAction(actions.DiscreteActions);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionOut = actionsOut.DiscreteActions;
            discreteActionOut[0] = Controller.GetCurrentAction();
            discreteActionOut[1] = Controller.GetCurrentPriority();
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            // var playerBehavior = playerInfoStorage.GetValue().GetPlayerInfo(_playerId).GetValue();
            for (int i = 0; i < gameBoard.GetValue().Count; i++)
            {
                if (gameBoard.GetValue()[i] != null)
                {
                    actionMask.SetActionEnabled(0, i,
                        gameBoard.GetValue()[i].GetPlayerId() != PlayerId);
                    // actionDisable += $"{i},";
                }
            }
            // Debug.Log(actionDisable);
        }
    }
}