using Unity.MLAgents.Actuators;
using UnityEngine;

namespace TheAiAlchemist
{
    // Use 1 branch action instead of 2 branches action 
    
    public class NpcPlayerGp2v2 : NpcPlayerGpTwo
    {
        public override void TakeAction(ActionSegment<int> action)
        {
            // Check if the plotSelecting is available: not at the same playerId
            // Check the prioritySelecting is at higher priority than the opponent
            // Remember to add 1 into action[1] to turn from zero-based space to one-based space
            var plotSelecting = action[0] / 3;
            var prioritySelecting = action[0] % 3;
            var plotValue = gameBoard.GetValue()[plotSelecting];
            // Debug.Log($"Player {_playerBehavior.GetPlayerId()}: Plot {plotSelecting}, priority {prioritySelecting}");
            if (_playerBehavior.GetInventory().IsProductAvailable(prioritySelecting) == false)
            {
                interruptGameChannel.ExecuteChannel();
                // Debug.Log($"Run out of circle: {prioritySelecting + 1}");
            }
            else if (plotValue == null)
            {
                _playerBehavior.GetInventory().Withdraw(prioritySelecting);
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(plotSelecting), prioritySelecting + 1);
            }
            else if (plotValue.GetPlayerId() == _playerBehavior.GetPlayerId())
            {
                interruptGameChannel.ExecuteChannel();
                // Debug.Log("Same player");
            }
            else if (prioritySelecting + 1 <= plotValue.GetPriority())
            {
                // Minus score for this one
                combatChannel.ExecuteChannel(_playerBehavior.GetPlayerId(),false);
                interruptGameChannel.ExecuteChannel();

                // Debug.Log($"Unavailable priority. Current: {plotValue.GetPriority()}, New: {prioritySelecting+1}");
            }
            else
            {
                // Add score for this one
                combatChannel.ExecuteChannel(_playerBehavior.GetPlayerId(),true);
                _playerBehavior.GetInventory().Withdraw(prioritySelecting);
                _playerBehavior.InTurnPlay(indexTranslator.IndexToPlot(plotSelecting), prioritySelecting + 1);
                // Debug.Log("Higher priority");
            }
        }
        
        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Z))
                currentAction = 0;
            else if (Input.GetKeyDown(KeyCode.X))
                currentAction = 3;
            else if (Input.GetKeyDown(KeyCode.C))
                currentAction = 6;
            else if (Input.GetKeyDown(KeyCode.A))
                currentAction = 9;
            else if (Input.GetKeyDown(KeyCode.S))
                currentAction = 12;
            else if (Input.GetKeyDown(KeyCode.D))
                currentAction = 15;
            else if (Input.GetKeyDown(KeyCode.Q))
                currentAction = 18;
            else if (Input.GetKeyDown(KeyCode.W))
                currentAction = 21;
            else if (Input.GetKeyDown(KeyCode.E))
                currentAction = 24;
            
            else if (Input.GetKeyDown(KeyCode.V))
                currentAction = 1;
            else if (Input.GetKeyDown(KeyCode.B))
                currentAction = 4;
            else if (Input.GetKeyDown(KeyCode.N))
                currentAction = 7;
            else if (Input.GetKeyDown(KeyCode.F))
                currentAction = 10;
            else if (Input.GetKeyDown(KeyCode.G))
                currentAction = 13;
            else if (Input.GetKeyDown(KeyCode.H))
                currentAction = 16;
            else if (Input.GetKeyDown(KeyCode.R))
                currentAction = 19;
            else if (Input.GetKeyDown(KeyCode.T))
                currentAction = 22;
            else if (Input.GetKeyDown(KeyCode.Y))
                currentAction = 25;
            
            else if (Input.GetKeyDown(KeyCode.M))
                currentAction = 2;
            else if (Input.GetKeyDown(KeyCode.Comma))
                currentAction = 5;
            else if (Input.GetKeyDown(KeyCode.Period))
                currentAction = 8;
            else if (Input.GetKeyDown(KeyCode.J))
                currentAction = 11;
            else if (Input.GetKeyDown(KeyCode.K))
                currentAction = 14;
            else if (Input.GetKeyDown(KeyCode.L))
                currentAction = 17;
            else if (Input.GetKeyDown(KeyCode.U))
                currentAction = 20;
            else if (Input.GetKeyDown(KeyCode.I))
                currentAction = 23;
            else if (Input.GetKeyDown(KeyCode.O))
                currentAction = 26;
        }
    }
}