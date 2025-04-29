using UnityEngine;
using UnityEngine.Localization;


namespace TheAiAlchemist
{
	[CreateAssetMenu(fileName = "SettingsSO", menuName = "TheAiAlchemist/Settings/SettingsSO")]

	public class SettingsSO : ScriptableObject
	{
		[SerializeField] private float _sfxVolume;
		public float SfxVolume => _sfxVolume;
		
		public void SaveAudioSettings(float newSfxVolume)
		{
			_sfxVolume = newSfxVolume;
		}

		public void LoadSavedSettings(Save savedFile)
		{
			_sfxVolume = savedFile._sfxVolume;
		}
	}
}
