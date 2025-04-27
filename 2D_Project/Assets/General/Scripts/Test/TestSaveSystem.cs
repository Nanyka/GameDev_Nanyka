using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace TheAiAlchemist
{
    public class TestSaveSystem : MonoBehaviour
    {
        [SerializeField] private IntChannel saveLevelChannel;
        [SerializeField] private SaveSystemManager saveSystemManager;
        
        [SerializeField] private bool hasSaveData;
        public string saveFilename = "save.nttt";
        public string backupSaveFilename = "save.nttt.bak";
        
        private void Awake()
        {
            saveSystemManager.Init(saveLevelChannel);
        }

        private void Start()
        {
            hasSaveData = saveSystemManager.LoadSaveDataFromDisk();
            if (hasSaveData == false)
            {
                saveSystemManager.WriteEmptySaveFile();
                saveSystemManager.SetNewGameData();
            }
            
            saveLevelChannel.ExecuteChannel(456);
            saveSystemManager.SaveDataToDisk();
        }
    }
}