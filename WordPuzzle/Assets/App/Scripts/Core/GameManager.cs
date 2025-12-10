using UnityEngine;
using WordPuzzle.Game.Model;
using WordPuzzle.UI;
using WordPuzzle.Game.Controllers;
using System.Collections;
using System.Collections.Generic;

namespace WordPuzzle.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public LevelData CurrentLevel { get; private set; }
        public int CurrentRackSize { get; private set; } = 5;

        [Header("Scene References")]
        [SerializeField] private RackController rackController;
        [SerializeField] private BoardController boardController;

        private readonly List<string> _wordHistory = new List<string>();
        private bool _endShown = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Initialize Systems
            WordDictionary.Instance.Initialize();
            
            // Show Start Screen
            UIManager.Instance.ShowScreen(GameScreen.StartMenu);
        }

        public void StartGame()
        {
            StartCoroutine(LoadLevelRoutine());
        }

        private IEnumerator LoadLevelRoutine()
        {
            UIManager.Instance.ShowScreen(GameScreen.Loading);
            yield return null; // Wait a frame

            // Generate Level (Heavy operation, might want to thread this actualy, but coroutine is ok for now)
            CurrentLevel = LevelGenerator.GenerateLevel(CurrentRackSize);

            _wordHistory.Clear();
            _endShown = false;
            if (CurrentLevel != null && !string.IsNullOrWhiteSpace(CurrentLevel.startWord))
            {
                _wordHistory.Add(CurrentLevel.startWord.ToUpper());
            }
            
            EventManager.TriggerEvent("LevelLoaded", CurrentLevel);
            UIManager.Instance.ShowScreen(GameScreen.GameHUD);
        }

        public void HandleWordAccepted(string lockedWord, List<TileData> remainingTiles, string playedWord)
        {
            if (CurrentLevel == null) return;
            if (_endShown) return; // prevent double-show
            if (!string.IsNullOrWhiteSpace(playedWord))
            {
                _wordHistory.Add(playedWord.ToUpper());
            }

            var remaining = remainingTiles ?? new List<TileData>();
            int tilesRemaining = remaining.Count;
            int possibleMoves = 0;

            var moves = GameRules.CalculatePossibleMoves(lockedWord.ToUpper(), remaining);
            foreach (var entry in moves.Values)
            {
                possibleMoves += entry.Count;
            }

            if (tilesRemaining == 0)
            {
                TriggerEndGame(EndGameResult.Win, tilesRemaining, possibleMoves);
                return;
            }

            if (possibleMoves == 0)
            {
                TriggerEndGame(EndGameResult.Lose, tilesRemaining, possibleMoves);
            }
        }

        public void NextLevel()
        {
            StartGame(); // Logic is same as start new game roughly
        }

        private void TriggerEndGame(EndGameResult result, int tilesRemaining, int possibleMoves)
        {
            if (_endShown) return;
            var payload = new EndGamePayload
            {
                result = result,
                wordsPlayed = new List<string>(_wordHistory),
                tilesRemaining = tilesRemaining,
                possibleMoves = possibleMoves,
                totalSolutions = CurrentLevel?.totalSolutions ?? 0,
                totalPaths = CurrentLevel?.totalPaths ?? 0
            };

            EventManager.TriggerEvent("EndGame", payload);
            _endShown = true;
        }
    }
}
