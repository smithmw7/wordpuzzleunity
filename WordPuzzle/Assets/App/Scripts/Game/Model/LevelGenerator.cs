using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordPuzzle.Game.Model
{
    public static class LevelGenerator
    {
        private const int WORD_LENGTH = 5;

        public static LevelData GenerateLevel(int targetRackSize = 5)
        {
            const int maxAttempts = 50;
            var wordList = WordDictionary.Instance.GetWordList();

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                string endWord = WordDictionary.Instance.GetRandomWord();
                string currentWord = endWord;
                
                var rackChars = new List<string>();
                var history = new HashSet<string> { endWord };
                var reverseSteps = new List<SolutionStep>();

                bool stuck = false;
                int steps = 0;

                // Step backward from End Word -> Start Word
                while (rackChars.Count < targetRackSize && steps < 20)
                {
                    steps++;
                    var neighbors = GetNeighbors(currentWord, history);
                    
                    // Shuffle neighbors
                    neighbors = neighbors.OrderBy(x => Random.value).ToList();

                    bool foundNext = false;
                    foreach (var nextWord in neighbors)
                    {
                        int diff = 0;
                        var newTiles = new List<string>();

                        for (int k = 0; k < WORD_LENGTH; k++)
                        {
                            if (currentWord[k] != nextWord[k])
                            {
                                diff++;
                                newTiles.Add(currentWord.Substring(k, 1)); // We use the char from the successor (current in loop, pre-step in time)
                            }
                        }

                        if (rackChars.Count + diff <= targetRackSize)
                        {
                            rackChars.AddRange(newTiles);
                            
                            reverseSteps.Add(new SolutionStep
                            {
                                fromWord = nextWord,
                                targetWord = currentWord,
                                tilesUsed = newTiles
                            });

                            currentWord = nextWord;
                            history.Add(currentWord);
                            foundNext = true;
                            break;
                        }
                    }

                    if (!foundNext)
                    {
                        stuck = true;
                        break;
                    }
                }

                if (!stuck && rackChars.Count == targetRackSize)
                {
                    // Convert basic chars to TileData
                    var rackTiles = rackChars.Select(c => new TileData(System.Guid.NewGuid().ToString(), c.ToUpper())).ToList();
                    reverseSteps.Reverse(); // Now it is Start -> End solution

                    return new LevelData
                    {
                        startWord = currentWord.ToUpper(),
                        endWord = endWord.ToUpper(),
                        rackTiles = rackTiles,
                        solution = reverseSteps
                    };
                }
            }

            return GenerateFallbackLevel(targetRackSize);
        }

        private static List<string> GetNeighbors(string word, HashSet<string> history)
        {
            var neighbors = new List<string>();
            var allWords = WordDictionary.Instance.GetWordList();
            
            // Optimization: Iterate all words is O(N*L), acceptable
            foreach (var w in allWords)
            {
                if (history.Contains(w)) continue;

                int diffCount = 0;
                for (int i = 0; i < WORD_LENGTH; i++)
                {
                    if (word[i] != w[i]) diffCount++;
                }

                if (diffCount >= 1 && diffCount <= 3)
                {
                    neighbors.Add(w);
                }
            }
            return neighbors;
        }

        private static LevelData GenerateFallbackLevel(int targetRackSize)
        {
            var fallbackRack = new List<TileData> { new TileData("1", "S") };
            while (fallbackRack.Count < targetRackSize)
            {
                char randomChar = (char)('A' + Random.Range(0, 26));
                fallbackRack.Add(new TileData(System.Guid.NewGuid().ToString(), randomChar.ToString()));
            }

            return new LevelData
            {
                startWord = "PLATE",
                endWord = "SLATE",
                rackTiles = fallbackRack,
                solution = new List<SolutionStep>
                {
                    new SolutionStep { fromWord = "PLATE", targetWord = "SLATE", tilesUsed = new List<string> { "S" } }
                }
            };
        }
    }
}
