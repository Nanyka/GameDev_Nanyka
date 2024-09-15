using Unity.MLAgents.Actuators;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TicTacToeAgentGp2v2 : TicTacToeAgentGpTwo
    {
        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var discreteActionOut = actionsOut.DiscreteActions;
            discreteActionOut[0] = Controller.GetCurrentAction();
        }

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            var playerBehavior = playerInfoStorage.GetValue().GetPlayerInfo(PlayerId).GetValue();
            for (int i = 0; i < gameBoard.GetValue().Count; i++)
            {
                if (gameBoard.GetValue()[i] == null)
                    continue;

                // Mask player's plots
                if (gameBoard.GetValue()[i].GetPlayerId() == PlayerId)
                {
                    for (int j = 0; j < 3; j++)
                        actionMask.SetActionEnabled(0, i * 3 + j, false);
                }
                else
                {
                    // Lock all priority lower than or equal opponent's priority
                    for (int j = 0; j <= gameBoard.GetValue()[i].GetPriority(); j++)
                    {
                        // Debug.Log($"Player {_playerBehavior.GetPlayerId()}, action {i * 3 + j} is False");
                        actionMask.SetActionEnabled(0,  i * 3 + j, false);
                    }
                }
            }

            // Lock priority left out of the inventory
            for (int j = 0; j < 3; j++)
            {
                if (playerBehavior.GetInventory().IsProductAvailable(j) == false)
                {
                    for (int i = 0; i < 9; i++)
                        actionMask.SetActionEnabled(0, i * 3 + j, false);
                }
            }
        }
    }
}