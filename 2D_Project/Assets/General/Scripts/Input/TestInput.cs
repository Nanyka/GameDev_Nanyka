using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class TestInput : MonoBehaviour
    {
        [SerializeField] private InputReaderSO inputReaderSo;

        private void OnEnable()
        {
            inputReaderSo.jumpEvent.AddListener(TestJumpPress);
            inputReaderSo.clickEvent.AddListener(PrintMousePosition);
        }

        private void OnDisable()
        {
            inputReaderSo.jumpEvent.RemoveListener(TestJumpPress);
        }

        private void TestJumpPress()
        {
            Debug.Log("Successful jump");
        }
        
        // Ex1: Detect mouse position
        private void PrintMousePosition(Vector3 arg0)
        {
            // print the position here --->
        }
        
    }
}