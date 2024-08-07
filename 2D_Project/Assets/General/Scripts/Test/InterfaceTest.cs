using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceTest : MonoBehaviour
{
    private void Start()
    {
        var entity = new Cat();
        
        // LetTalk(cat);
    }

    private void LetTalk(ITalk whoTalk)
    {
        whoTalk.Talk();
    }
}