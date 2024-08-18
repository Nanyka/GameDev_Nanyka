using Unity.MLAgents.Actuators;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TicTacToeAgentGp2v2 : TicTacToeAgentGpTwo
    {
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionOut = actionsOut.DiscreteActions;
            discreteActionOut[0] = _controller.GetCurrentAction();
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            for (int i = 0; i < gameBoard.GetValue().Count; i++)
            {
                if (gameBoard.GetValue()[i] == null)
                {
                    // Lock priority left out of the inventory
                    for (int j = 0; j < 3; j++)
                    {
                        // Debug.Log($"Player {_playerBehavior.GetPlayerId()}, action {i * 3 + j} is {_inventory.IsProductAvailable(j)}");
                        actionMask.SetActionEnabled(0, i * 3 + j, _inventory.IsProductAvailable(j));
                    }
                }
                else
                {
                    if (gameBoard.GetValue()[i].GetPlayerId() == _playerBehavior.GetPlayerId())
                    {
                        for (int j = 0; j < 3; j++)
                            actionMask.SetActionEnabled(0, i * 3 + j,false);
                    }
                    else
                    {
                        // Lock all priority lower than opponent's priority
                        var selectedPlot = gameBoard.GetValue()[i];
                        for (int j = 0; j < selectedPlot.GetPriority(); j++)
                        {
                            // Debug.Log($"Player {_playerBehavior.GetPlayerId()}, action {i * 3 + j} is False");
                            actionMask.SetActionEnabled(0, i * 3 + j, false);
                        }
                        
                        // Lock priority left out of the inventory
                        for (int j = selectedPlot.GetPriority(); j < 3; j++)
                        {
                            // Debug.Log($"Player {_playerBehavior.GetPlayerId()}, action {i * 3 + j} is {_inventory.IsProductAvailable(j)}");
                            actionMask.SetActionEnabled(0, i * 3 + j, _inventory.IsProductAvailable(j));
                        }
                    }
                }
            }
        }
    }
}