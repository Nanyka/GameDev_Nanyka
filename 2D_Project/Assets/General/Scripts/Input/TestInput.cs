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
        [FormerlySerializedAs("_inputReader")] [SerializeField] private InputReaderSO inputReaderSo;

        private void OnEnable()
        {
            inputReaderSo.jumpEvent.AddListener(TestJumpPress);
        }

        private void OnDisable()
        {
            inputReaderSo.jumpEvent.RemoveListener(TestJumpPress);
        }

        private void TestJumpPress()
        {
            Debug.Log("Successful jump");
        }
    }
}