using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EventScriptableObject eventScriptableObject;

    private FirstPlayerController firstPlayerController;
    private SecondPlayerController secondPlayerController;

    static bool isPlayer1 = true;

    private void Awake()
    {
        firstPlayerController = new FirstPlayerController();
        secondPlayerController = new SecondPlayerController();
    }

    private void OnEnable()
    {
        eventScriptableObject.eventChangePlayer.AddListener(ChangePlayer);
        eventScriptableObject.eventTouchItem.AddListener(OnTouchItem);
    }

    private void OnDisable()
    {
        eventScriptableObject.eventChangePlayer.RemoveListener(ChangePlayer);
        eventScriptableObject.eventTouchItem.RemoveListener(OnTouchItem);
    }

    public void ChangePlayer()
    {
        isPlayer1 = !isPlayer1;

        Debug.Log(isPlayer1 ? "turn of Player1" : "turn of Player2");
    }

    public static bool IsPlayer1()
    {
        return isPlayer1;
    }

    private void OnTouchItem()
    {
        if(isPlayer1)
        {
            PlayerTalk(firstPlayerController);
        }
        else
        {
            PlayerTalk(secondPlayerController);
        }
    }

    private void PlayerTalk(IPlayerBehavior playerBehavior)
    {
        playerBehavior.PlayerTalk();
    }
}
