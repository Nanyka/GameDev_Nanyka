using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// create a variable to contain a EventObject
// listen to the event from the object by a specific function
// generate a function to print out the received position

public class Circle : MonoBehaviour
{
    [SerializeField] private Vector3Channel positionCircle;
    [SerializeField] private SpriteRenderer m_Renderer;

    private void OnEnable()
    {
        positionCircle.Vector3Event.AddListener(DetectTouchPoint);
    }

    private void OnDisable()
    {
        positionCircle.Vector3Event.RemoveListener(DetectTouchPoint);
    }

    private void DetectTouchPoint(Vector3 inputPosition)
    {
        // check the position of the circle and inputPosition
        // just enable the renderer when player touch on the circle
        var myPosition = transform.position;

        float distance = Vector3.Distance(inputPosition, myPosition);

        if(distance < 0.5)
        {
            m_Renderer.enabled = true;
        }
        
    }
    
}
