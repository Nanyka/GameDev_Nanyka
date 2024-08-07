using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

// create a variable to contain a EventObject
// listen to the event from the object by a specific function
// generate a function to print out the received position

namespace TheAiAlchemist
{
    public class Circle : MonoBehaviour, ICircleTrait
    {
        // [SerializeField] private Vector3Channel mousePosChannel;
        // [SerializeField] private SpriteRenderer m_Renderer;

        private ICheckState mState;
        private IRender mRenderer;
        [SerializeField] private int circleId;

        // private void OnEnable()
        // {
        //     mousePosChannel.AddListener(DetectTouchPoint);
        // }
        //
        // private void OnDisable()
        // {
        //     mousePosChannel.RemoveListener(DetectTouchPoint);
        // }

        public void Init(Vector3 spawnPos)
        {
            mState = GetComponent<ICheckState>();
            mRenderer = GetComponent<IRender>();

            transform.position = spawnPos;
            circleId = Mathf.RoundToInt(spawnPos.x + spawnPos.y * 3 + 4); //TODO: replace hard coding by a flexible grid size
        }

        public bool DetectTouchPoint(Vector3 inputPosition)
        {
            // check the position of the circle and inputPosition
            float distance = Vector3.Distance(inputPosition, transform.position);
            return distance < 0.5;
        }

        public int GetId()
        {
            return circleId;
        }

        // public void ChangeState()
        // {
        //     // change state component of the circle
        //     // update circle's render component
        //     mState.ChangeState();
        //     mRenderer.ActivateRenderer(mState.IsActivated());
        // }
    }
}