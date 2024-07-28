using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MouseEvent", menuName = "ScriptableObject/MouseEvent")]
public class MouseEvent : ScriptableObject
{
    public UnityEvent<Vector3> eventMouseTouch;

    public void RunEventMouseTouch(Vector3 mouseWorldPosition)
    {
        eventMouseTouch?.Invoke(mouseWorldPosition);
    }
}
