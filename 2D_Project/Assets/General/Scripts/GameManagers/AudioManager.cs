using System;
using UnityEngine;

namespace TheAiAlchemist
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private FloatChannel changeSfxVolumeChannel;
        [SerializeField] private IntChannel audioPlayIndex;
        [SerializeField] private AudioClip[] soundClips;

        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            audioPlayIndex.AddListener(PlaySound);
            changeSfxVolumeChannel.AddListener(ChangeSfx);
        }

        private void OnDisable()
        {
            audioPlayIndex.RemoveListener(PlaySound);
            changeSfxVolumeChannel.RemoveListener(ChangeSfx);
        }

        private void PlaySound(int soundIndex)
        {
            // Debug.Log($"Play sound {soundIndex}");
            audioSource.resource = soundClips[soundIndex];
            audioSource.Play();
        }

        private void ChangeSfx(float sfxVolume)
        {
            audioSource.volume = sfxVolume;
        }
    }
}