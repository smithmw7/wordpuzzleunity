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
            var puzzle = WordDictionary.Instance.CurrentPuzzle;
            if (puzzle == null)
            {
                Debug.LogError("Puzzle data missing. Ensure puzzles_mostsolutions.json is in Resources.");
                return null;
            }

            if (puzzle.rack == null || puzzle.rack.Length == 0)
            {
                Debug.LogError("Puzzle rack is empty or missing.");
                return null;
            }

            var rackTiles = puzzle.rack
                .Take(targetRackSize)
                .Select(letter => new TileData(System.Guid.NewGuid().ToString(), letter.ToUpper()))
                .ToList();

            return new LevelData
            {
                startWord = puzzle.start.ToUpper(),
                endWord = puzzle.start.ToUpper(),
                rackTiles = rackTiles,
                solution = new List<SolutionStep>(),
                totalSolutions = puzzle.total_solutions,
                totalPaths = puzzle.total_paths
            };
        }
    }
}
