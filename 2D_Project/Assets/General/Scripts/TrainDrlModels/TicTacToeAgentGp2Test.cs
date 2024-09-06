using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace TheAiAlchemist
{
    public class TicTacToeAgentGp2Test : TicTacToeAgentGp2v2
    {
        [SerializeField] private VoidChannel resetMask;
        [SerializeField] private IntChannel visualizeMask;

        public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
        {
            resetMask.ExecuteChannel();

            // for (int i = 0; i < gameBoard.GetValue().Count; i++)
            // {
            //     if (gameBoard.GetValue()[i] == null)
            //     {
            //         // Lock priority left out of the inventory
            //         for (int j = 0; j < 3; j++)
            //         {
            //             // Debug.Log($"Player {_playerBehavior.GetPlayerId()}, action {i * 3 + j} is {_inventory.IsProductAvailable(j)}");
            //             actionMask.SetActionEnabled(0, i * 3 + j, _playerBehavior.GetInventory().IsProductAvailable(j));
            //             if (_playerBehavior.GetInventory().IsProductAvailable(j) == false)
            //                 visualizeMask.ExecuteChannel(i * 3 + j);
            //         }
            //     }
            //     else
            //     {
            //         if (gameBoard.GetValue()[i].GetPlayerId() == _playerBehavior.GetPlayerId())
            //         {
            //             for (int j = 0; j < 3; j++)
            //             {
            //                 actionMask.SetActionEnabled(0, i * 3 + j, false);
            //                 visualizeMask.ExecuteChannel(i * 3 + j);
            //             }
            //         }
            //         else
            //         {
            //             // Lock all priority lower than opponent's priority
            //             var selectedPlot = gameBoard.GetValue()[i];
            //             var maskOpponent = $"At {i}: ";
            //             for (int j = 0; j <= selectedPlot.GetPriority(); j++)
            //             {
            //                 // Debug.Log($"Player {_playerBehavior.GetPlayerId()}, action {i * 3 + j} is False");
            //                 var maskIndex = i * 3 + j;
            //                 actionMask.SetActionEnabled(0, maskIndex, false);
            //                 if (_playerBehavior.GetInventory().IsProductAvailable(selectedPlot.GetPriority()+1) == false)
            //                     visualizeMask.ExecuteChannel(maskIndex);
            //                 maskOpponent += $"{maskIndex}, ";
            //             }
            //             Debug.Log(maskOpponent);
            //         }
            //     }
            // }

            for (int i = 0; i < gameBoard.GetValue().Count; i++)
            {
                if (gameBoard.GetValue()[i] == null)
                    continue;

                if (gameBoard.GetValue()[i].GetPlayerId() == _playerBehavior.GetPlayerId())
                {
                    for (int j = 0; j < 3; j++)
                    {
                        actionMask.SetActionEnabled(0, i * 3 + j, false);
                        visualizeMask.ExecuteChannel(i * 3 + j);
                    }
                }
                else
                {
                    // Lock all priority lower than opponent's priority
                    var selectedPlot = gameBoard.GetValue()[i];
                    var maskOpponent = $"At {i}: ";
                    for (int j = 0; j <= selectedPlot.GetPriority(); j++)
                    {
                        // Debug.Log($"Player {_playerBehavior.GetPlayerId()}, action {i * 3 + j} is False");
                        var maskIndex = i * 3 + j;
                        actionMask.SetActionEnabled(0, maskIndex, false);
                        if (_playerBehavior.GetInventory().IsProductAvailable(selectedPlot.GetPriority() + 1) == false)
                            visualizeMask.ExecuteChannel(maskIndex);
                        maskOpponent += $"{maskIndex}, ";
                    }

                    Debug.Log(maskOpponent);
                }
            }

            // Lock priority left out of the inventory
            for (int j = 0; j < 3; j++)
            {
                if (_playerBehavior.GetInventory().IsProductAvailable(j) == false)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        actionMask.SetActionEnabled(0, i * 3 + j, false);
                        visualizeMask.ExecuteChannel(i * 3 + j);
                    }
                }
            }

            Debug.Log($"Masked!!");
        }
    }
}