using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;


namespace TheAiAlchemist
{
	[CreateAssetMenu(fileName = "SettingsSO", menuName = "TheAiAlchemist/Settings/SettingsSO")]

	public class SettingsSO : ScriptableObject
	{
		[SerializeField] private float sfxVolume;
		[SerializeField] private float musicVolume;

		public float SfxVolume => sfxVolume;
		public float MusicVolume => musicVolume;

		public void SaveAudioSettings(float newSfxVolume, float songVolume)
		{
			sfxVolume = newSfxVolume;
			musicVolume = songVolume;
		}

		public void LoadSavedSettings(Save savedFile)
		{
			sfxVolume = savedFile.sfxVolume;
			musicVolume = savedFile.musicVolume;
		}
	}
}
