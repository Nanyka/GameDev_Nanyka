using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Nana/Settings/InputReader")]
public class InputReaderSO : ScriptableObject, MyInput.IMacOSActions

{
    public UnityEvent jumpEvent;
    public UnityEvent mousePositionEvent;

    private MyInput myInput;

    private void OnEnable()
    {
        if(myInput == null)
        {
            myInput = new MyInput();
            myInput.MacOS.SetCallbacks(this);
        }
        myInput.MacOS.Enable();
    }

    private void OnDisable()
    {
        myInput.MacOS.Disable();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (jumpEvent != null && context.phase == InputActionPhase.Performed)
        {
            jumpEvent.Invoke();
        }
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        if (mousePositionEvent != null && context.phase == InputActionPhase.Performed)
        {
            mousePositionEvent.Invoke();
            Debug.Log($"Touch position:{Touchscreen.current.primaryTouch.position.ReadValue()}");
        }
    }
}
