using System;
using System.Collections.Generic;

namespace WordPuzzle.Game.Model
{
    [Serializable]
    public class TileData
    {
        public string id;
        public string charValue;

        public TileData(string id, string charValue)
        {
            this.id = id;
            this.charValue = charValue;
        }
    }

    public enum SlotState
    {
        Empty,
        Locked,
        Staged
    }

    [Serializable]
    public class BoardSlot
    {
        public int index;
        public string lockedChar; // The character that is permanent (background)
        public TileData stagedTile; // The character placed by the user (nullable)

        public BoardSlot(int index, string lockedChar)
        {
            this.index = index;
            this.lockedChar = lockedChar;
            this.stagedTile = null;
        }
    }

    [Serializable]
    public struct SolutionStep
    {
        public string fromWord;
        public string targetWord;
        public List<string> tilesUsed;
    }

    [Serializable]
    public class LevelData
    {
        public string startWord;
        public string endWord;
        public List<TileData> rackTiles;
        public List<SolutionStep> solution;

        // Debug/meta data surfaced from the puzzle JSON
        public int totalSolutions;
        public int totalPaths;
    }
}
