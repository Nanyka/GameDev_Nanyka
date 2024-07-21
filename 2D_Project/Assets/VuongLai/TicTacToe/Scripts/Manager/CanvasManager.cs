using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] UIBase currentMenu;
    [SerializeField] EventScriptableObject eventScriptableObject;

    public void RunEventChangePlayer()
    {
        eventScriptableObject.RunEventChangePlayer();
    }
}
