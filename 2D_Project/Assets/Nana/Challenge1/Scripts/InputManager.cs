using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// define mouse's position when mouse button down
// transform that position from screen space to word space via scriptable object event
// asign the position to the game object (cirle have a component to print this position)

namespace Nana
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Vector3Channel mousePosChannel; 

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePosition = Input.mousePosition;
                var wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                wordPosition = new Vector3(wordPosition.x, wordPosition.y, 0f);
                mousePosChannel.ExecuteChannel(wordPosition);
            }
        }
    }
}
