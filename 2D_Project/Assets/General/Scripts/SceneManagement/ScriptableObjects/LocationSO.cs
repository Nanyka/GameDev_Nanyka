using UnityEngine;
using UnityEngine.Localization;

namespace TheAiAlchemist
{
	/// <summary>
	/// This class contains Settings specific to Locations only
	/// </summary>

	[CreateAssetMenu(fileName = "NewLocation", menuName = "TheAiAlchemist/SceneData/Location")]
	public class LocationSO : GameSceneSO
	{
		public LocalizedString locationName;
	}
}
