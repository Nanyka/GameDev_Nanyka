using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EventScriptableObject", menuName = "ScriptableObject/EventScriptableObject")]
public class EventScriptableObject : ScriptableObject
{
    public UnityEvent eventChangePlayer;
    public UnityEvent eventTouchItem;

    public void RunEventChangePlayer()
    {
        eventChangePlayer?.Invoke();
    }

    public void RunEventTouchItem()
    {
        eventTouchItem?.Invoke();
    }
}
