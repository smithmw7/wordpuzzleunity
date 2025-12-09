using UnityEngine;
using WordPuzzle.Game.Model;
using WordPuzzle.UI;
using System.Collections;

namespace WordPuzzle.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public LevelData CurrentLevel { get; private set; }
        public int CurrentRackSize { get; private set; } = 5;

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

        private void OnEnable()
        {
            EventManager.StartListening("WordSubmitted", OnWordSubmitted);
        }

        private void OnDisable()
        {
            EventManager.StopListening("WordSubmitted", OnWordSubmitted);
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
            
            EventManager.TriggerEvent("LevelLoaded", CurrentLevel);
            UIManager.Instance.ShowScreen(GameScreen.GameHUD);
        }

        private void OnWordSubmitted(object payload)
        {
            // Logic: The BoardController has already validated the word and consumed tiles.
            // We just need to check if we won (Rack Empty).
            
            // We can query the RackController or track it ourselves. 
            // Since we are the source of truth for "CurrentLevel", we know how many tiles started.
            // But tracking consumed tiles is cleaner via event.
            // Let's assume for now if a valid word is submitted that uses tiles, we check if rack is empty.
            
            // Actually, best way: RackController should broadcast "RackEmpty" or we check it.
            // Let's rely on a helper or find object for now to keep it simple as requested.
            
            var rack = FindObjectOfType<WordPuzzle.Game.Controllers.RackController>();
            if (rack != null && rack.transform.childCount == 0) // transform.childCount includes the tiles
            {
                // Wait a moment for visual effect?
                StartCoroutine(WinRoutine());
            }
        }

        private IEnumerator WinRoutine()
        {
            EventManager.TriggerEvent("LevelWon", null);
            yield return new WaitForSeconds(0.5f);
            LevelWon();
        }

        public void LevelWon()
        {
             UIManager.Instance.ShowScreen(GameScreen.WinModal);
        }

        public void NextLevel()
        {
            StartGame(); // Logic is same as start new game roughly
        }
    }
}
