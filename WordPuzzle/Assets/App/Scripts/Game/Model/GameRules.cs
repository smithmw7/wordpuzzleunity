using System.Collections.Generic;
using System.Linq;

namespace WordPuzzle.Game.Model
{
    public static class GameRules
    {
        public const int WORD_LENGTH = 5;

        public static bool IsWordValid(string word)
        {
            return WordDictionary.Instance.IsWordValid(word);
        }

        /// <summary>
        /// Calculates possible words that can be formed from the current word by replacing characters 
        /// with available tiles from the rack.
        /// </summary>
        public static Dictionary<int, List<string>> CalculatePossibleMoves(string currentWord, List<TileData> rackTiles)
        {
            var rackCounts = new Dictionary<char, int>();
            foreach (var t in rackTiles)
            {
                char c = char.ToUpper(t.charValue[0]);
                if (!rackCounts.ContainsKey(c)) rackCounts[c] = 0;
                rackCounts[c]++;
            }

            var movesByTileCount = new Dictionary<int, List<string>>();
            string upperCurrentWord = currentWord.ToUpper();
            var allWords = WordDictionary.Instance.GetWordList();

            foreach (var word in allWords)
            {
                string targetWord = word.ToUpper();
                if (targetWord == upperCurrentWord) continue;

                var tempRack = new Dictionary<char, int>(rackCounts);
                bool possible = true;
                int tilesUsed = 0;

                for (int i = 0; i < WORD_LENGTH; i++)
                {
                    if (targetWord[i] != upperCurrentWord[i])
                    {
                        // Need tile
                        char needed = targetWord[i];
                        if (tempRack.ContainsKey(needed) && tempRack[needed] > 0)
                        {
                            tempRack[needed]--;
                            tilesUsed++;
                        }
                        else
                        {
                            possible = false;
                            break;
                        }
                    }
                }

                if (possible && tilesUsed > 0)
                {
                    if (!movesByTileCount.ContainsKey(tilesUsed))
                    {
                        movesByTileCount[tilesUsed] = new List<string>();
                    }
                    movesByTileCount[tilesUsed].Add(targetWord);
                }
            }

            foreach (var key in movesByTileCount.Keys.ToList())
            {
                movesByTileCount[key].Sort();
            }

            return movesByTileCount;
        }
    }
}
