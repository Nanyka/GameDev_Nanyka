using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputSystem inputSystem;
        [SerializeField] private V_Vector3Channel touchItemChannel;

        [Header("Config")]
        [SerializeField] private Vector2 minPivot;
        [SerializeField] private Vector2 maxPivot;
        [SerializeField] private float stepVertical;
        [SerializeField] private float stepHorizontal;

        private List<Vector2> hasItemPosition = new List<Vector2>();

        private void OnEnable()
        {
            inputSystem.eventMouseTouch.AddListener(TouchItem);
        }

        private void OnDisable()
        {
            inputSystem.eventMouseTouch.RemoveListener(TouchItem);
        }

        private Vector3? GetItemPosition(Vector3 touchPosition)
        {
            float halfStepVertical = Mathf.Abs(stepVertical) / 2;
            float halfStepHorizontal = Mathf.Abs(stepHorizontal) / 2;

            if (touchPosition.x < (minPivot.x - halfStepHorizontal) || touchPosition.x > (maxPivot.x + halfStepHorizontal)
                || touchPosition.y < (minPivot.y - halfStepVertical) || touchPosition.y > (maxPivot.y + halfStepVertical))
            {
                Debug.Log("Out Space");
                return null;
            }

            float positionX = 0;
            if (touchPosition.x < minPivot.x)
            {
                positionX = minPivot.x;
            }
            else if (touchPosition.x > maxPivot.x)
            {
                positionX = maxPivot.x;
            }
            else
            {
                float distance = touchPosition.x - minPivot.x;
                int indexX = (int)(distance / stepHorizontal);

                if (touchPosition.x - ((indexX * stepHorizontal) + minPivot.x) > halfStepHorizontal)
                {
                    indexX++;
                }

                positionX = (indexX * stepHorizontal) + minPivot.x;
            }

            float positionY = 0;
            if (touchPosition.y < minPivot.y)
            {
                positionY = minPivot.y;
            }
            else if (touchPosition.y > maxPivot.y)
            {
                positionY = maxPivot.y;
            }
            else
            {
                float distance = touchPosition.y - minPivot.y;
                int indexY = (int)(distance / stepVertical);

                if (touchPosition.y - ((indexY * stepVertical) + minPivot.y) > halfStepVertical)
                {
                    indexY++;
                }

                positionY = (indexY * stepVertical) + minPivot.y;
            }

            Vector2 itemPosition = new Vector2(positionX, positionY);

            if(HasItem(itemPosition))
            {
                return null;
            }

            hasItemPosition.Add(itemPosition);

            return itemPosition;
        }

        private bool HasItem(Vector2 position)
        {
            return hasItemPosition.Contains(position);
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
    }
}