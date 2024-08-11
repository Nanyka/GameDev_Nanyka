using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace V_TicTacToe
{
    public class MouseInput : MonoBehaviour
    {
        [SerializeField] MouseEvent mouseEvent;

        private void Update()
        {
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                var mousePosition = Mouse.current.position.ReadValue();
                var wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                wordPosition = new Vector3(wordPosition.x, wordPosition.y, 0f);
                mouseEvent.eventMouseTouch.Invoke(wordPosition);
            }
        }
    }
}