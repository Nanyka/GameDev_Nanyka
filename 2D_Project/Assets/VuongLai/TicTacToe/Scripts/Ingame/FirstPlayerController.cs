using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerController : MonoBehaviour, IPlayerBehavior
{
    public void PlayerTalk()
    {
        Debug.Log("I am FirstPlayer");
    }
}
