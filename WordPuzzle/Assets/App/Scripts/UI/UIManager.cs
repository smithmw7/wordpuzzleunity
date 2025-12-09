using System.Collections.Generic;
using UnityEngine;

namespace WordPuzzle.UI
{
    public enum GameScreen
    {
        StartMenu,
        Loading,
        GameHUD,
        WinModal
    }

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Screens")]
        public GameObject startMenuPanel;
        public GameObject loadingPanel;
        public GameObject gameHUDPanel;
        public GameObject winModalPanel;

        private Dictionary<GameScreen, GameObject> _panels;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            
            _panels = new Dictionary<GameScreen, GameObject>
            {
                { GameScreen.StartMenu, startMenuPanel },
                { GameScreen.Loading, loadingPanel },
                { GameScreen.GameHUD, gameHUDPanel },
                { GameScreen.WinModal, winModalPanel }
            };
        }

        public void ShowScreen(GameScreen screen)
        {
            // Hide all
            foreach(var panel in _panels.Values)
            {
                if(panel != null) panel.SetActive(false);
            }

            // Show target
            if (_panels.TryGetValue(screen, out var targetPanel) && targetPanel != null)
            {
                targetPanel.SetActive(true);
            }
        }
        
        // Button Callbacks linked in Inspector
        public void OnStartGameButton()
        {
            WordPuzzle.Core.GameManager.Instance.StartGame();
        }

        public void OnNextLevelButton()
        {
            WordPuzzle.Core.GameManager.Instance.NextLevel();
        }
    }
}
