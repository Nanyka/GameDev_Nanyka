using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondPlayerController : MonoBehaviour, IPlayerBehavior
{
    public void PlayerTalk()
    {
        Debug.Log("I am SecondPlayer");
    }
}
