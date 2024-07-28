using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerController : MonoBehaviour, IPlayerBehavior
{
    [SerializeField] EventScriptableObject eventScriptableObject;
    [SerializeField] PlayerInfor playerInfor;

    private void Awake()
    {
        playerInfor.SetCurrentPlayerBehavior(this);
    }

    private void OnEnable()
    {
        eventScriptableObject.eventChangePlayer.AddListener(OnChangePlayer);
    }

    private void OnDisable()
    {
        eventScriptableObject.eventChangePlayer.RemoveListener(OnChangePlayer);
    }

    void OnChangePlayer()
    {
        if (playerInfor.IsFirstPlayer())
        {
            playerInfor.SetCurrentPlayerBehavior(this);
        }
    }

    public void PlayerTalk()
    {
        Debug.Log("I am FirstPlayer");
    }
}
