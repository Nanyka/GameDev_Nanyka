using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // [SerializeField] private EventScriptableObject eventScriptableObject;
    [SerializeField] private VoidChannel changePlayerChannel;
    [SerializeField] private VoidChannel touchItemChannel;
    [SerializeField] private BooleanStorage isPlayerStorage;
    [SerializeField] private IPlayerBehaviorStorage player1;
    [SerializeField] private IPlayerBehaviorStorage player2;
    [SerializeField] private PlayerInfor playerInfor;
    [SerializeField] private PlayerEvent playerEvent;

    private void OnEnable()
    {
        // eventScriptableObject.eventChangePlayer.AddListener(ChangePlayer);
        // eventScriptableObject.eventTouchItem.AddListener(OnTouchItem);
        
        // changePlayerChannel.channel.AddListener(ChangePlayer);
        touchItemChannel.channel.AddListener(OnTouchItem);
    }

    private void OnDisable()
    {
        // eventScriptableObject.eventChangePlayer.RemoveListener(ChangePlayer);
        // eventScriptableObject.eventTouchItem.RemoveListener(OnTouchItem);
        
        // changePlayerChannel.channel.RemoveListener(ChangePlayer);
        touchItemChannel.channel.RemoveListener(OnTouchItem);
    }

    public void ChangePlayer()
    {
        //playerInfor.ChangePlayer();

        // Debug.Log(playerInfor.IsFirstPlayer() ? "turn of Player1" : "turn of Player2");
    }

    private void OnTouchItem()
    {
        if (isPlayerStorage == true)
            player1.value.PlayerTalk();
        else
            player2.value.PlayerTalk();
        
        // playerInfor.RunEventCurrentPlayerTalk();
    }
}
