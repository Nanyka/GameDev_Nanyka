using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// define mouse's position when mouse button down
// transform that position from screen space to word space via scriptable object event
// assign the position to the game object (circle have a component to print this position)

namespace TheAiAlchemist
{
    public class InputManager : MonoBehaviour
    {
        // [SerializeField] private Vector3Channel mousePosChannel; 
        //
        // private void Update()
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         var mousePosition = Input.mousePosition;
        //         var wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //         wordPosition = new Vector3(Mathf.RoundToInt(wordPosition.x), Mathf.RoundToInt(wordPosition.y), 0f);
        //         mousePosChannel.ExecuteChannel(wordPosition);
        //     }
        // }

        [SerializeField] private InputReaderSO inputReaderSo;
        [SerializeField] private Vector3Channel mousePosChannel;

        private Camera mainCamera;

        private void OnEnable()
        {
            inputReaderSo.clickEvent.AddListener(PrintMousePosition);
            mainCamera = Camera.main;
        }

        private void OnDisable()
        {
            inputReaderSo.clickEvent.RemoveListener(PrintMousePosition);
        }

        private void PrintMousePosition(Vector3 clickPoint)
        {
            if (PointingChecker.IsPointerOverUIObject() == false)
            {
                var wordPosition = mainCamera.ScreenToWorldPoint(clickPoint);
                wordPosition = new Vector3(Mathf.RoundToInt(wordPosition.x), Mathf.RoundToInt(wordPosition.y), 0f);
                if (Mathf.Abs(wordPosition.x + wordPosition.y) > 2)
                    Debug.Log("You are clicking outside the board");
                else
                    mousePosChannel.ExecuteChannel(wordPosition);
            }
        }
    }
}