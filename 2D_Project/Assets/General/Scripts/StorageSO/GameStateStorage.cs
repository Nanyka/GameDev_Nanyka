using AlphaZeroAlgorithm;
using UnityEngine;
using UnityEngine.Events;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "GameStateStorage", menuName = "TheAiAlchemist/Storages/GameStateStorage")]
    public class GameStateStorage : ScriptableObject
    {
        private GameState value;
        
        public void SetValue(GameState value)
        {
            this.value = value;
        }

        public GameState GetValue()
        {
            return value;
        }
    }
}