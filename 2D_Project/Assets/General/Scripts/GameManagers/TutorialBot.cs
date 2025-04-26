using System.Collections.Generic;
using System.Threading.Tasks;
using AlphaZeroAlgorithm;
using UnityEngine;

namespace TheAiAlchemist
{
    public class 
        TutorialBot: IAgent
    {
        private Move[] moves;
        private int currentIndex;

        public void Init()
        {
            moves = new[] { new Move(new Point(2, 2, 1)), new Move(new Point(1, 3, 3)) };
        }
        
        public Task<Move> SelectMove(GameState gameState)
        {
            if (currentIndex >= moves.Length)
                return null;
            
            return Task.FromResult(moves[currentIndex++]);
        }

        public Move GetMoveFromInput(string inputString)
        {
            return null;
        }
    }
}