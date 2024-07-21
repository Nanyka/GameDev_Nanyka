using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] EventScriptableObject eventScriptableObject;

    static bool isPlayer1 = true;

    private void OnEnable()
    {
        eventScriptableObject.eventChangePlayer.AddListener(ChangePlayer);
    }

    private void OnDisable()
    {
        eventScriptableObject.eventChangePlayer.RemoveListener(ChangePlayer);
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
}
