using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JumpeeIsland;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using Unity.Services.Economy;
using UnityEngine;

namespace TheAiAlchemist
{
    /// <summary>
    /// This class connect Remote Config service with the game
    /// </summary>
    [CreateAssetMenu(fileName = "CloudSaveManager", menuName = "TheAiAlchemist/BackEnd/CloudSaveManager")]
    public class CloudSaveManagerSO : ScriptableObject
    {
        const string k_GameProcess = "GAME_PROCESS";
        const string k_Models = "MODELS";
        const string k_Materials = "MATERIALS";
        private Dictionary<string, string> _cloudSaveData;
        
        // CLOUD SAVE - GAME DATA
        
        

        // public async Task Init()
        // {
        //     await FetchPlayerCloudSave();
        // }
        //
        // private async Task FetchPlayerCloudSave()
        // {
        //     try
        //     {
        //         _cloudSaveData = await CloudSaveService.Instance.Data.LoadAllAsync();
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogException(e);
        //     }
        // }
        

        // DATA
        // public async Task SaveGameProcess(GameProcessData currentProcess)
        // {
        //     try
        //     {
        //         var dataToSave = new Dictionary<string, object>
        //         {
        //             { k_GameProcess, currentProcess }
        //         };
        //
        //         await CloudSaveService.Instance.Data.Player.SaveAsync(dataToSave);
        //     }
        //     catch (Exception e)
        //     {
        //         Debug.LogException(e);
        //     }
        // }
        //
        // public GameProcessData FetchGameProcess()
        // {
        //     return _cloudSaveData.TryGetValue(k_GameProcess, out var value)
        //         ? JsonUtility.FromJson<GameProcessData>(value)
        //         : null;
        // }
    }
}