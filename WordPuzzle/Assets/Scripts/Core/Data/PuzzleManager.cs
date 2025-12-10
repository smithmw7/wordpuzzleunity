using System.Collections.Generic;
using UnityEngine;
using WordPuzzle.Game.Model;

namespace WordPuzzle.Core.Data
{
    public class PuzzleManager : MonoBehaviour
    {
        public static PuzzleManager Instance { get; private set; }

        private List<LevelData> _puzzleBank5 = new List<LevelData>();
        private List<LevelData> _puzzleBank7 = new List<LevelData>();
        private const int BANK_SIZE = 10; // Keep smaller for mobile mem

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void EnsureBank(int rackSize)
        {
            var bank = rackSize == 7 ? _puzzleBank7 : _puzzleBank5;
            
            if (bank.Count < BANK_SIZE)
            {
                // Fill up bank asynchronously ideally, but blocking for now is OK for this loop
                int needed = BANK_SIZE - bank.Count;
                for (int i = 0; i < needed; i++)
                {
                    bank.Add(LevelGenerator.GenerateLevel(rackSize));
                }
            }
        }

        public LevelData GetNextPuzzle(int rackSize)
        {
            EnsureBank(rackSize);
            var bank = rackSize == 7 ? _puzzleBank7 : _puzzleBank5;

            if (bank.Count > 0)
            {
                // Pop from front
                var level = bank[0];
                bank.RemoveAt(0);
                
                // Trigger refill in background?
                // For now, let's just refill next time Ensure is called or lazily
                return level; 
            }

            // Fallback
            return LevelGenerator.GenerateLevel(rackSize);
        }

        // Allow regenerating if user gets stuck/debug
        public LevelData RegenerateLevel(int rackSize)
        {
            return LevelGenerator.GenerateLevel(rackSize);
        }
    }
}
