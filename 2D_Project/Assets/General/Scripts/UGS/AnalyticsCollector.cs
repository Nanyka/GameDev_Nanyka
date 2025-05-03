using System;
using AlphaZeroAlgorithm;
using Unity.Services.Analytics;
using UnityEngine;

namespace TheAiAlchemist
{
    public class AnalyticsCollector: MonoBehaviour
    {
        [SerializeField] private BooleanStorage ugsInitialized;
        [SerializeField] private GameStateStorage gameStateStorage;
        [SerializeField] private BoolChannel endGameChannel;
        [SerializeField] private SaveSystemManager saveSystemManager;

        private void OnEnable()
        {
            endGameChannel.AddListener(RecordData);
        }

        private void OnDisable()
        {
            endGameChannel.RemoveListener(RecordData);
        }

        private void RecordData(bool isWinner)
        {
            if (ugsInitialized.GetValue() == false) return;
            
            int gameResult = 0;
            if (isWinner) gameResult = gameStateStorage.GetValue().Winner() == Player.X ? 1 : -1;
            
            GameResult myEvent = new GameResult
            {
                WinningResult = gameResult,
                BotLevel = saveSystemManager.saveData.level
            };
                
            AnalyticsService.Instance.RecordEvent(myEvent);
        }
    }
    
    public class GameResult : Unity.Services.Analytics.Event
    {
        public GameResult() : base("gameResult")
        {
        }

        public int WinningResult { set { SetParameter("winningResult", value); } }
        public int BotLevel { set { SetParameter("botLevel", value); } }
    }
}