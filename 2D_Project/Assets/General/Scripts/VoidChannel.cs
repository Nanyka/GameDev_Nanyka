using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidChannel", menuName = "TheAiAlchemist/Channels/VoidChannel")]
public class VoidChannel : ScriptableObject
{
    public UnityEvent channel;
}
