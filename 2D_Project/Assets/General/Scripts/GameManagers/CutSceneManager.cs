using System;
using UnityEngine;
using UnityEngine.Playables;

namespace TheAiAlchemist
{
    public class CutSceneManager : MonoBehaviour
    {
        [SerializeField] private TimelineManager timelineManager;

        private PlayableDirector playableDirector;


        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
        }

        private void Start()
        {
            timelineManager.Init();
            playableDirector.Play();
        }
    }
}