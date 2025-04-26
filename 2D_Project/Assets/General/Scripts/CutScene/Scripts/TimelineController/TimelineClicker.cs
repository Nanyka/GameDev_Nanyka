using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class TimelineClicker : MonoBehaviour
    {
        [SerializeField] private ButtonRequire buttonRequire;
        [SerializeField] private TimelineManager timelineManager;
        [SerializeField] private int clickableCount;

        public void OnClick()
        {
            if (clickableCount > 0)
            {
                timelineManager.ResumeTimeline(buttonRequire);
                if (clickableCount < 99)
                    clickableCount--;
            }
        }
    }
}
