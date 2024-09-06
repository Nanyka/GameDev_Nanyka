using System;
using System.Collections;
using System.Collections.Generic;
using TheAiAlchemist;
using UnityEngine;

public class AskActionButton : MonoBehaviour
{
    [SerializeField] private NpcChannel WaitForAction;
    [SerializeField] private ListCircleStorage gameBoard;

    private INpcPlayer currentNpc;

    private void OnEnable()
    {
        WaitForAction.AddListener(SetCurrentNpc);
    }

    private void OnDisable()
    {
        WaitForAction.RemoveListener(SetCurrentNpc);
    }

    private void SetCurrentNpc(INpcPlayer npc)
    {
        currentNpc = npc;

        // var checkGameBoard = "GameBoard: ";
        // foreach (var circleTrait in gameBoard.GetValue())
        //     if (circleTrait != null)
        //         checkGameBoard += $"{circleTrait.GetId()}:{circleTrait.GetPlayerId()}, ";
        // Debug.Log(checkGameBoard);
    }

    public void OnAskForAction()
    {
        currentNpc.AskForAction();
    }
}