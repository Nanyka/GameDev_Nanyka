using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TheAiAlchemist
{
    public abstract class PointingChecker
    {
        public static bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            // eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            eventDataCurrentPosition.position = Mouse.current.position.value;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
