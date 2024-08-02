using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private V_VoidChannel changePlayerChannel;
    [SerializeField] private V_VoidChannel touchItemChannel;
    [SerializeField] private V_BooleanStorage isFirstPlayer;
    [SerializeField] private V_IPlayerBehaviorStorage player1;
    [SerializeField] private V_IPlayerBehaviorStorage player2;

    private void Awake()
    {
        isFirstPlayer.SetValue(true);
    }

    private void OnEnable()
    {   
        changePlayerChannel.AddListener(ChangePlayer);
        touchItemChannel.AddListener(OnTouchItem);
    }

    private void OnDisable()
    {
        changePlayerChannel.RemoveListener(ChangePlayer);
        touchItemChannel.RemoveListener(OnTouchItem);
    }

    public void ChangePlayer()
    {
        Debug.Log(isFirstPlayer.GetValue() ? "turn of Player1" : "turn of Player2");
    }

    private void OnTouchItem()
    {
        if (isFirstPlayer.GetValue() == true)
            player1.GetValue().PlayerTalk();
        else
            player2.GetValue().PlayerTalk();
    }
}
