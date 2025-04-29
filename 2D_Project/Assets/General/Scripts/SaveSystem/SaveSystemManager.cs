using System.Collections;
using System.Collections.Generic;
using TheAiAlchemist;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "SaveSystemManager", menuName = "TheAiAlchemist/BackEnd/SaveSystemManager")]

public class SaveSystemManager : ScriptableObject
{
	public string saveFilename = "save.nttt";
	public string backupSaveFilename = "save.nttt.bak";
	public Save saveData = new();
	
	private IntChannel saveLevel;

	public void Init(IntChannel levelChannel)
	{
		DiscardAllDescriptions();
		saveLevel = levelChannel;
		saveLevel.AddListener(SaveLevel);
	}

	private void DiscardAllDescriptions()
	{
		if (saveLevel != null)
			saveLevel.RemoveAllListener();
	}

	private void SaveLevel(int currentLevel)
	{
		saveData.SaveLevel(currentLevel);
	}

	private void CacheLoadLocations(GameSceneSO locationToLoad, bool showLoadingScreen, bool fadeScreen)
	{
		LocationSO locationSO = locationToLoad as LocationSO;
		if (locationSO)
		{
			// saveData._locationId = locationSO.Guid;
		}

		SaveDataToDisk();
	}

	public bool LoadSaveDataFromDisk()
	{
		if (FileManager.LoadFromFile(saveFilename, out var json))
		{
			saveData.LoadFromJson(json);
			return true;
		}

		return false;
	}

	public void SaveDataToDisk()
	{
		if (FileManager.MoveFile(saveFilename, backupSaveFilename))
		{
			if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
			{
				//Debug.Log("Save successful " + saveFilename);
			}
		}
	}

	public void WriteEmptySaveFile()
	{
		FileManager.WriteToFile(saveFilename, "");
	}
	public void SetNewGameData()
	{
		FileManager.WriteToFile(saveFilename, "");

		SaveDataToDisk();

	}
	public void SaveSettings(SettingsSO settings)
	{
		saveData.SaveSettings(settings);
		SaveDataToDisk();
	}
}
