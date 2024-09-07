using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private V_Vector3Channel touchItemChannel;
        [SerializeField] private V_Vector2Storage currentMatrixPosition;
        [SerializeField] private V_VoidChannel endTurnChannel;
        [SerializeField] private V_VoidChannel resetLevelChannel;
        [SerializeField] private V_IntegerStorage currentNumber;

        [Header("Config")]
        [SerializeField] private Vector2 matrixNumber;
        [SerializeField] private Vector2 minPivot;
        [SerializeField] private Vector2 maxPivot;
        [SerializeField] private float stepVertical;
        [SerializeField] private float stepHorizontal;

        private List<Vector2> hasItemPosition = new List<Vector2>();
        private Vector2 matrixPosition = Vector2.zero;

        private void OnEnable()
        {
            inputSystem.eventMouseTouch.AddListener(TouchItem);
            endTurnChannel.AddListener(OnEndTurn);
            resetLevelChannel.AddListener(OnResetLevel);
        }

        private void OnDisable()
        {
            inputSystem.eventMouseTouch.RemoveListener(TouchItem);
            endTurnChannel.RemoveListener(OnEndTurn);
            resetLevelChannel.RemoveListener(OnResetLevel);
        }

        private Vector3? GetItemPosition(Vector3 touchPosition)
        {
            int currentNumber = -1;

            float halfStepVertical = Mathf.Abs(stepVertical) / 2;
            float halfStepHorizontal = Mathf.Abs(stepHorizontal) / 2;

            if (touchPosition.x < (minPivot.x - halfStepHorizontal) || touchPosition.x > (maxPivot.x + halfStepHorizontal)
                || touchPosition.y < (minPivot.y - halfStepVertical) || touchPosition.y > (maxPivot.y + halfStepVertical))
            {
                matrixPosition = new Vector2(-1, -1);
                currentMatrixPosition.Value = matrixPosition;

                this.currentNumber.Value = currentNumber;

                Debug.Log("Out Space");
                Debug.Log($"CurrentNumber: {currentNumber}");
                return null;
            }

            float positionX = 0;
            float matrixPositionX = 0;
            int xIndex = 0;
            if (touchPosition.x < minPivot.x)
            {
                positionX = minPivot.x;
               
            }
            else if (touchPosition.x > maxPivot.x)
            {
                positionX = maxPivot.x;
                matrixPositionX = matrixNumber.x - 1;

                xIndex = (int)(matrixNumber.x - 1);
            }
            else
            {
                float distance = touchPosition.x - minPivot.x;
                int indexX = (int)(distance / stepHorizontal);

                if (touchPosition.x - ((indexX * stepHorizontal) + minPivot.x) > halfStepHorizontal)
                {
                    indexX++;
                }

                matrixPositionX = indexX;

                xIndex = indexX;

                positionX = (indexX * stepHorizontal) + minPivot.x;
            }

            float positionY = 0;
            float matrixPositionY = 0;
            int yIndex = 0;
            if (touchPosition.y < minPivot.y)
            {
                positionY = minPivot.y;
                matrixPositionY = matrixNumber.y - 1;
            }
            else if (touchPosition.y > maxPivot.y)
            {
                positionY = maxPivot.y;

                yIndex = (int)(matrixNumber.y - 1);
            }
            else
            {
                float distance = touchPosition.y - minPivot.y;
                int indexY = (int)(distance / stepVertical);

                if (touchPosition.y - ((indexY * stepVertical) + minPivot.y) > halfStepVertical)
                {
                    indexY++;
                }

                matrixPositionY = matrixNumber.y - (indexY) - 1;
                yIndex = indexY;

                positionY = (indexY * stepVertical) + minPivot.y;
            }

            Vector2 itemPosition = new Vector2(positionX, positionY);
            matrixPosition = new Vector2(matrixPositionX, matrixPositionY);
            currentMatrixPosition.Value = matrixPosition;

            currentNumber = xIndex + (yIndex * (int)matrixNumber.x);
            this.currentNumber.Value = currentNumber;

            if(HasItem(matrixPosition))
            {
                return null;
            }

            return itemPosition;
        }

        private bool HasItem(Vector2 position)
        {
            return hasItemPosition.Contains(position);
        }

        private void OnEndTurn()
        {
            hasItemPosition.Add(currentMatrixPosition.Value);
        }

        private void TouchItem(Vector3 touchPosition)
        {
            Vector3? valueItemPosition = GetItemPosition(touchPosition);
            if (valueItemPosition != null)
            {
                Vector3 itemPosition = valueItemPosition.Value;
                touchItemChannel.RunVector3Channel(itemPosition);
            }

        }

        private void OnResetLevel()
        {
            hasItemPosition.Clear();
        }
    }

    [System.Serializable]
    public class MatrixPosition
    {
        public int matrixX = 0;
        public int matrixY = 0;
    }
}