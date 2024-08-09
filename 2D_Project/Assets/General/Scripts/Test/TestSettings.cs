using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This settings activate InputReader when developer using it to test GamePlayScene

namespace TheAiAlchemist
{
    public class TestSettings : MonoBehaviour
    {
        [SerializeField] private VoidChannel activateInput;

        private void Start()
        {
            activateInput.ExecuteChannel();
        }
    }
}
