using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItem : MonoBehaviour
{
    [SerializeField] private EventScriptableObject eventScriptable;
    [SerializeField] private GameObject checkIconObject;
    [SerializeField] bool isFirstPlayerCheck;
    [SerializeField] private MouseEvent mouseEvent;
    [SerializeField] private PlayerInfor playerInfor;

    bool isChecked;

    private void Awake()
    {
        checkIconObject.SetActive(false);
    }

    private void OnEnable()
    {
        mouseEvent.eventMouseTouch.AddListener(DetectShowCheck);
    }

    private void OnDisable()
    {
        mouseEvent.eventMouseTouch.RemoveListener(DetectShowCheck);
    }

    private void OnMouseDown()
    {
        //eventScriptable.RunEventTouchItem();
    }

    private void DetectShowCheck(Vector3 mouseWorldPosition)
    {
        if (playerInfor.IsFirstPlayer() != isFirstPlayerCheck)
        {
            return;
        }

        if (isChecked)
        {
            return;
        }

        Vector3 myPosition = transform.position;

        float distance = Vector3.Distance(myPosition, mouseWorldPosition);

        if (distance <= 0.5f)
        {
            isChecked = true;
            checkIconObject.SetActive(true);
        }
    }
}
