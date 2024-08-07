using UnityEngine;

// create a variable to contain a EventObject
// listen to the event from the object by a specific function
// generate a function to print out the received position

namespace Nana
{
    public class Circle : MonoBehaviour
    {
        [SerializeField] private Vector3Channel mousePosChannel;
        [SerializeField] private SpriteRenderer m_Renderer;

        private void OnEnable()
        {
            mousePosChannel.AddListener(DetectTouchPoint);
        }

        private void OnDisable()
        {
            mousePosChannel.RemoveListener(DetectTouchPoint);
        }
        
        private void DetectTouchPoint(Vector3 inputPosition)
        {
            // check the position of the circle and inputPosition
            // change state component of the circle
            // update circle's render component
            var myPosition = transform.position;

            float distance = Vector3.Distance(inputPosition, myPosition);

            if (distance < 0.5)
            {
                m_Renderer.enabled = true;
            }
        }
    }
}