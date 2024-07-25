using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItem : MonoBehaviour
{
    [SerializeField] private EventScriptableObject eventScriptable;

    private void OnMouseDown()
    {
        eventScriptable.RunEventTouchItem();
    }
}
