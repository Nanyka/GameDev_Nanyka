using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace TheAiAlchemist
{

    [Serializable]
    public class DialogueClip : PlayableAsset, ITimelineClipAsset
    {
        public DialogueBehaviour template = new();

        public ClipCaps clipCaps
        {
            get { return ClipCaps.None; }
        }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<DialogueBehaviour>.Create(graph, template);

            return playable;
        }
    }
}
