using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Nana/Vector3Channel")]
public class Vector3Channel : ScriptableObject
{
    public UnityEvent<Vector3> Vector3Event;
}
