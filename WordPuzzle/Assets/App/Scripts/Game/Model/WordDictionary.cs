using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WordPuzzle.Game.Model
{
    public class WordDictionary
    {
        private static WordDictionary _instance;
        public static WordDictionary Instance => _instance ??= new WordDictionary();

        private HashSet<string> _validWords;
        private List<string> _wordList;
        private PuzzleDataRecord _puzzle;
        private List<PuzzleDataRecord> _allPuzzles;
        private int _currentPuzzleIndex = 0;

        public bool IsInitialized => _validWords != null;
        public PuzzleDataRecord CurrentPuzzle => _puzzle;

        public void Initialize()
        {
            if (IsInitialized) return;

            TextAsset puzzleFile = Resources.Load<TextAsset>("puzzles_mostsolutions");
            if (puzzleFile == null)
            {
                Debug.LogError("Could not find puzzles_mostsolutions.json in Resources!");
                return;
            }

            var parsed = JsonUtility.FromJson<PuzzleDataRoot>(puzzleFile.text);
            _allPuzzles = parsed?.puzzles ?? new List<PuzzleDataRecord>();
            
            if (_allPuzzles.Count == 0)
            {
                Debug.LogError("No puzzles found in puzzles_mostsolutions.json!");
                return;
            }

            _currentPuzzleIndex = 0;
            _puzzle = _allPuzzles[_currentPuzzleIndex];

            if (_puzzle == null || string.IsNullOrWhiteSpace(_puzzle.start) || _puzzle.rack == null)
            {
                Debug.LogError("Failed to parse puzzle data or puzzle is missing required fields.");
                return;
            }

            TextAsset wordFile = Resources.Load<TextAsset>("words");
            if (wordFile == null)
            {
                Debug.LogError("Could not find words.txt in Resources!");
                return;
            }

            _wordList = ParseWords(wordFile.text);
            _validWords = new HashSet<string>(_wordList, System.StringComparer.OrdinalIgnoreCase);

            Debug.Log($"WordDictionary loaded {_validWords.Count} words from words.txt.");
        }

        public bool IsWordValid(string word)
        {
            if (!IsInitialized) Initialize();
            return _validWords.Contains(word.ToLower());
        }

        public List<string> GetWordList()
        {
            if (!IsInitialized) Initialize();
            return _wordList;
        }

        public string GetRandomWord()
        {
             if (!IsInitialized) Initialize();
             return _wordList[Random.Range(0, _wordList.Count)];
        }

        public bool LoadNextPuzzle()
        {
            if (_allPuzzles == null || _allPuzzles.Count == 0)
            {
                Debug.LogError("No puzzles available to load.");
                return false;
            }

            _currentPuzzleIndex = (_currentPuzzleIndex + 1) % _allPuzzles.Count;
            _puzzle = _allPuzzles[_currentPuzzleIndex];

            if (_puzzle == null || string.IsNullOrWhiteSpace(_puzzle.start) || _puzzle.rack == null)
            {
                Debug.LogError($"Puzzle at index {_currentPuzzleIndex} is invalid.");
                return false;
            }

            Debug.Log($"Loaded puzzle {_currentPuzzleIndex + 1}/{_allPuzzles.Count}: {_puzzle.start}");
            return true;
        }

        private static List<string> ParseWords(string raw)
        {
            return raw
                .Split((char[])null, System.StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim().ToLower())
                .Where(w => w.Length == GameRules.WORD_LENGTH)
                .Distinct()
                .ToList();
        }
    }
}
