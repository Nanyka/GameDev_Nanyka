using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTester : MonoBehaviour
{
    public void OnJump(InputAction.CallbackContext content)
    {
        if (content.phase == InputActionPhase.Performed)
        {
            Debug.Log("I am Jump");
        }
    }
}
