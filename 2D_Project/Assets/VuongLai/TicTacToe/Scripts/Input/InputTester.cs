using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTester : MonoBehaviour
{
    [SerializeField] private InputSystem inputSystem;

    private void OnEnable()
    {
        inputSystem.eventJump.AddListener(OnJump);
    }

    private void OnDisable()
    {
        inputSystem.eventJump.RemoveListener(OnJump);
    }

    public void OnJump(InputAction.CallbackContext content)
    {
        if (content.phase == InputActionPhase.Performed)
        {
            Debug.Log("I am Jump");
        }
    }
}
