using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JumpeeIsland;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "TimelineManager", menuName = "TheAiAlchemist/Settings/TimelineManager")]
    public class TimelineManager : ScriptableObject
    {
        [HideInInspector]
        public UnityEvent<PlayableDirector, ButtonRequire>
            OnPauseTimeline; // send to this, TimelineCameraMover; invoke at DialogBehaviour

        [HideInInspector]
        public UnityEvent<string, ButtonRequire>
            OnStringCheck; // send to StringChecker, invoke at AoeTutorialConstructingButton

        private PlayableDirector _activeDirector;
        private ButtonRequire? _curButtonRequire;
        private bool _isPause;

        public void Init()
        {
            OnPauseTimeline.RemoveAllListeners();
            OnPauseTimeline.AddListener(PauseTimeline);
            _isPause = false;
        }

        public void PauseTimeline(PlayableDirector whichOne, ButtonRequire buttonRequire)
        {
            if (_isPause == false)
            {
                _activeDirector = whichOne;
                if (_activeDirector.playableGraph.IsValid() == false)
                    return;

                _curButtonRequire = buttonRequire;
                _activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(0d);
                _isPause = true;
            }
        }

        public void ResumeTimeline(ButtonRequire buttonRequire)
        {
            if (_isPause && (buttonRequire == _curButtonRequire || _curButtonRequire == ButtonRequire.NONE))
            {
                if (_activeDirector.playableGraph.IsValid() == false)
                    return;

                var trackAsset = ((TimelineAsset)_activeDirector.playableAsset).GetOutputTracks()
                    .First(e => e.name.Equals("Dialogue Track"));
                if (trackAsset != null)
                {
                    var bindingObject = _activeDirector.GetGenericBinding(trackAsset) as DialogUI;
                    if (bindingObject != null) bindingObject.ToggleDialogBox(false);
                }

                _activeDirector.playableGraph.GetRootPlayable(0).SetSpeed(1d);
                _isPause = false;
                // _curButtonRequire = null;
            }
        }
    }
}