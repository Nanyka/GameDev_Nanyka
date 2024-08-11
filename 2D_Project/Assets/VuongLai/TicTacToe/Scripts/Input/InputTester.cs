using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace V_TicTacToe
{
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

        public void OnJump()
        {
            Debug.Log("I am Jump");
        }
    }
}