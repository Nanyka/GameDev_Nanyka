using System;
using AlphaZeroAlgorithm;
using UnityEngine;

namespace TheAiAlchemist
{
    public class TutorialPlot : Plot, IClickable
    {
        [SerializeField] private TimelineClicker timelineClicker;

        protected override void OnMouseUpAsButton()
        {
            if (askUnitIndex.GetValue() < 0)
            {
                Debug.Log("Need to select a unit");
            }
            else if (askUnitIndex.GetValue() <= currentStrength)
            {
                Debug.Log("Invalid move with strength lower than current strength");
            }
            else
            {
                currentStrength = askUnitIndex.GetValue();
                selectPointChannel.ExecuteChannel(new Point(row,col,currentStrength));
                timelineClicker.OnClick();
            }
        }
        
        public void OnListenClick()
        {
            OnMouseUpAsButton();
        }
    }
}