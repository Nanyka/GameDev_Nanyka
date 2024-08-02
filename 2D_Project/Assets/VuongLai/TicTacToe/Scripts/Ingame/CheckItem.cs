using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckItem : MonoBehaviour
{
    [SerializeField] private GameObject checkIconObject;
    [SerializeField] bool isFirstPlayerCheck;
    [SerializeField] private MouseEvent mouseEvent;
    [SerializeField] private V_VoidChannel touchItemChannel;
    [SerializeField] private V_BooleanStorage isFirstPlayer;
    [SerializeField] private InputSystem inputSystem;

    bool isChecked;

    private void Awake()
    {
        checkIconObject.SetActive(false);
    }

    private void OnEnable()
    {
        mouseEvent.eventMouseTouch.AddListener(DetectShowCheck);

        inputSystem.eventMouseTouch.AddListener(DetectShowCheck);
    }

    private void OnDisable()
    {
        mouseEvent.eventMouseTouch.RemoveListener(DetectShowCheck);

        inputSystem.eventMouseTouch.RemoveListener(DetectShowCheck);
    }

    private void OnMouseDown()
    {
        //eventScriptable.RunEventTouchItem();
    }

    private void DetectShowCheck(Vector3 mouseWorldPosition)
    {
        if (isFirstPlayer.GetValue() != isFirstPlayerCheck)
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

            touchItemChannel.RunVoidChannel();
        }
    }
}
