using System;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TutorialClicker: MonoBehaviour
    {
        private IClickable parentClickable;

        private void Start()
        {
            parentClickable = GetComponentInParent<IClickable>();
        }

        private void OnMouseUpAsButton()
        {
            parentClickable.OnListenClick();
        }
    }
}