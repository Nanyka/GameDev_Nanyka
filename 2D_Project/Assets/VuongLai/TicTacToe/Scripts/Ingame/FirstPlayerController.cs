using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerController : MonoBehaviour, IPlayerBehavior
{
    [SerializeField] private IPlayerBehaviorStorage m_PlayerBehavior;
    // [SerializeField] EventScriptableObject eventScriptableObject;
    // [SerializeField] PlayerInfor playerInfor;

    private void Awake()
    {
        m_PlayerBehavior.value = this;
    }

    // private void Awake()
    // {
    //     playerInfor.SetIsFirstPlayer();
    //     playerInfor.SetCurrentPlayerBehavior(this);
    // }

    // private void OnEnable()
    // {
    //     eventScriptableObject.eventChangePlayer.AddListener(ChangePlayer);
    // }
    //
    // private void OnDisable()
    // {
    //     eventScriptableObject.eventChangePlayer.RemoveListener(ChangePlayer);
    // }
    //
    // public void ChangePlayer()
    // {
    //     if (playerInfor.IsFirstPlayer())
    //     {
    //         playerInfor.SetCurrentPlayerBehavior(this);
    //     }
    // }

    public void PlayerTalk()
    {
        Debug.Log("I am FirstPlayer");
    }
}
