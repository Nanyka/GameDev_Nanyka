using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// define mouse's position when mouse button down
// transform that position from screen space to word space via scriptable object event
// assign the position to the game object (circles have a component to print this position out)

namespace TheAiAlchemist
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputReaderSO inputReaderSo;
        [SerializeField] private Vector3Channel mousePosChannel;
        [SerializeField] private VoidChannel activateInput;

        private Camera mainCamera;

        private void OnEnable()
        {
            inputReaderSo.clickEvent.AddListener(PrintMousePosition);
            activateInput.AddListener(ActivateInputReader);
        }

        private void OnDisable()
        {
            inputReaderSo.clickEvent.RemoveListener(PrintMousePosition);
            activateInput.RemoveListener(ActivateInputReader);
        }

        private void ActivateInputReader()
        {
            mainCamera = Camera.main;
            inputReaderSo.EnableGameplayInput();
        }

        private void PrintMousePosition(Vector3 clickPoint)
        {
            if (mainCamera == null)
                return;
            
            if (PointingChecker.IsPointerOverUIObject() == false)
            {
                var wordPosition = mainCamera.ScreenToWorldPoint(clickPoint);
                wordPosition = new Vector3(Mathf.RoundToInt(wordPosition.x), Mathf.RoundToInt(wordPosition.y), 0f);
                if (Mathf.Abs(wordPosition.x) + Mathf.Abs(wordPosition.y) < 2)
                    mousePosChannel.ExecuteChannel(wordPosition);
            }
        }
    }
}