using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "InputSystem", menuName = "ScriptableObject/InputSystem")]
    public class InputSystem : ScriptableObject, MyInput.IMacOsActions
    {
        private MyInput myInput;

        private InputAction mousePositionAction;

        public UnityEvent eventJump;
        public UnityEvent<Vector3> eventMouseTouch;

        private bool mouseTouch;

        private void OnEnable()
        {
            if (myInput == null)
            {
                myInput = new MyInput();
                myInput.MacOs.SetCallbacks(this);
            }

            myInput.MacOs.Enable();

            mousePositionAction = myInput.FindAction("MousePosition");
        }

        private void OnDisable()
        {
            myInput.MacOs.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (eventJump != null
                && context.phase == InputActionPhase.Performed)
            {
                eventJump?.Invoke();
            }
        }

        public void OnMouseTouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                mouseTouch = true;

                if (eventMouseTouch != null)
                {
                    mouseTouch = false;

                    Vector2 mousePosition = mousePositionAction.ReadValue<Vector2>();

                    var wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    wordPosition = new Vector3(wordPosition.x, wordPosition.y, 0f);
                    eventMouseTouch?.Invoke(wordPosition);
                }
            }
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {

        }
    }
}