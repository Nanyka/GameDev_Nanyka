using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName ="InputSystem", menuName ="ScriptableObject/InputSystem")]
public class InputSystem : ScriptableObject, MyInput.IMacOsActions
{
    private MyInput myInput;

    public UnityEvent<InputAction.CallbackContext> eventJump;

    public void OnJump(InputAction.CallbackContext context)
    {
        if(eventJump!=null
            &&context.phase==InputActionPhase.Performed)
        {
            eventJump.Invoke(context);
        }
    }

    private void OnEnable()
    {
        if(myInput==null)
        {
            myInput = new MyInput();
            myInput.MacOs.SetCallbacks(this);
        }
    }
}
