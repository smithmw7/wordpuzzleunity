using System;
using System.Collections.Generic;

namespace WordPuzzle.Game.Model
{
    /// <summary>
    /// Minimal shape for a puzzle entry coming from puzzles_mostsolutions.json.
    /// Extra fields in the JSON are ignored by JsonUtility.
    /// </summary>
    [Serializable]
    public class PuzzleDataRecord
    {
        public string start;
        public string[] rack;
        public int total_solutions;
        public int total_paths;

        // Solution paths grouped by length. Only the words themselves matter for now.
        public List<string[]> S_1;
        public List<string[]> S_2;
        public List<string[]> S_3;
        public List<string[]> S_4;
        public List<string[]> S_5;
    }
}
