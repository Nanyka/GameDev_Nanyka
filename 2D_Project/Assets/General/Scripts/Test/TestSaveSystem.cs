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
        public string saveFilename = "save.chop";
        public string backupSaveFilename = "save.chop.bak";
        
        private void Awake()
        {
            saveSystemManager.Init(saveLevelChannel);
        }

        private void Start()
        {
            
            // saveSystemManager.WriteEmptySaveFile();
            // saveSystemManager.SetNewGameData();
            
            // saveLevelChannel.ExecuteChannel(123);
            // saveSystemManager.SaveDataToDisk();
            
            hasSaveData = saveSystemManager.LoadSaveDataFromDisk();

        }
    }
}