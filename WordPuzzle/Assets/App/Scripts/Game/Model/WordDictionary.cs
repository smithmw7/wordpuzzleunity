using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text.RegularExpressions;

namespace WordPuzzle.Game.Model
{
    public class WordDictionary
    {
        private static WordDictionary _instance;
        public static WordDictionary Instance => _instance ??= new WordDictionary();

        private HashSet<string> _validWords;
        private List<string> _wordList;

        public bool IsInitialized => _validWords != null;

        public void Initialize()
        {
            if (IsInitialized) return;

            TextAsset wordFile = Resources.Load<TextAsset>("words");
            if (wordFile == null)
            {
                Debug.LogError("Could not find words.txt in Resources!");
                return;
            }

            var rawInfo = wordFile.text;
            // Split by newlines and trim
            var words = Regex.Split(rawInfo, @"\s+").Where(w => !string.IsNullOrWhiteSpace(w)).Select(w => w.Trim().ToLower()).ToList();

            // Filter for length 5 just in case
            _wordList = words.Where(w => w.Length == 5).ToList();
            _validWords = new HashSet<string>(_wordList);

            Debug.Log($"WordDictionary loaded {_validWords.Count} words.");
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
    }
}
