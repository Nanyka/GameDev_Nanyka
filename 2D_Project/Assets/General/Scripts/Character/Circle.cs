using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private IndexAndPlotTranslator indexTranslator;
        [SerializeField] private Vector3Storage gameBoardPos;
        [SerializeField] private TextMeshProUGUI circlePriority;
        
        private ICheckState mState;
        private IRender mRenderer;
        [SerializeField] private int mPlayerId;
        private int mCircleId;
        private int mPriority;

        public void Init(Vector3 spawnPos, int playerId, int priority)
        {
            mState = GetComponent<ICheckState>();
            mRenderer = GetComponent<IRender>();
            transform.position = spawnPos + gameBoardPos.GetValue();
            circlePriority.text = (priority + 1).ToString();
            mPlayerId = playerId;
            mCircleId = indexTranslator.PlotToIndex(spawnPos);
            mPriority = priority;
        }

        public bool DetectTouchPoint(Vector3 inputPosition)
        {
            // check the position of the circle and inputPosition
            float distance = Vector3.Distance(inputPosition, transform.position);
            return distance < 0.5;
        }

        public int GetPlayerId()
        {
            return mPlayerId;
        }

        public int GetId()
        {
            return mCircleId;
        }

        public int GetPriority()
        {
            return mPriority;
        }

        public void DisableCircle()
        {
            gameObject.SetActive(false);
        }
    }
}