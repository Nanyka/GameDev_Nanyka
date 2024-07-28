using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EventScriptableObject eventScriptableObject;
    [SerializeField] private PlayerInfor playerInfor;
    [SerializeField] private PlayerEvent playerEvent;

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
        //playerInfor.ChangePlayer();

        Debug.Log(playerInfor.IsFirstPlayer() ? "turn of Player1" : "turn of Player2");
    }

    private void OnTouchItem()
    {
        playerInfor.RunEventCurrentPlayerTalk();
    }
}
