using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInput : MonoBehaviour
{
    [SerializeField] private InputReaderSO inputReaderSo;

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
