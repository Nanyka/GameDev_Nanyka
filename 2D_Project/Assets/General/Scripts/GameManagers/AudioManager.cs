using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private VoidChannel changeSettingsChannel;
        [SerializeField] private SettingsSO settingsSo;
        [SerializeField] private IntChannel audioPlayIndex;
        [SerializeField] private AudioClip[] soundClips;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        private void OnEnable()
        {
            audioPlayIndex.AddListener(PlaySfx);
            changeSettingsChannel.AddListener(ChangeSfx);
        }

        private void OnDisable()
        {
            audioPlayIndex.RemoveListener(PlaySfx);
            changeSettingsChannel.RemoveListener(ChangeSfx);
        }

        private void Start()
        {
            musicSource.Play();
        }

        private void PlaySfx(int soundIndex)
        {
            sfxSource.resource = soundClips[soundIndex];
            sfxSource.Play();
        }

        private void ChangeSfx()
        {
            sfxSource.volume = settingsSo.SfxVolume;
            musicSource.volume = settingsSo.MusicVolume;
        }
    }
}