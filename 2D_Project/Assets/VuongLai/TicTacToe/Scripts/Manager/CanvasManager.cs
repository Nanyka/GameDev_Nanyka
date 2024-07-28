using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] UIBase currentMenu;
    [SerializeField] EventScriptableObject eventScriptableObject;
    [SerializeField] PlayerInfor playerInfor;

    public void RunEventChangePlayer()
    {
        playerInfor.ChangePlayer();

        eventScriptableObject.RunEventChangePlayer();
    }
}
