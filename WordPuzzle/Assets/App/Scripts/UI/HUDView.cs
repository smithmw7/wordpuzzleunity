using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ensure we use TMP as requested
using WordPuzzle.Game.Controllers;

namespace WordPuzzle.UI
{
    public class HUDView : MonoBehaviour
    {
        [Header("References")]
        public Button shuffleButton;
        public Button submitButton;
        public TextMeshProUGUI tilesLeftText; // Example of feedback

        private BoardController _boardController;
        private RackController _rackController;

        private void Start()
        {
            // Auto-find controllers if not assigned (Decoupling: UI finds what it needs or is injected)
            _boardController = FindObjectOfType<BoardController>();
            _rackController = FindObjectOfType<RackController>();

            if (shuffleButton != null)
                shuffleButton.onClick.AddListener(OnShuffleClicked);
            
            if (submitButton != null)
                submitButton.onClick.AddListener(OnSubmitClicked);
        }

        private void OnShuffleClicked()
        {
            if (_rackController != null) _rackController.Shuffle();
        }

        private void OnSubmitClicked()
        {
            if (_boardController != null) _boardController.SubmitWord();
        }
    }
}
