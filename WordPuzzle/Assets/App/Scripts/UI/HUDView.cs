using UnityEngine;
using UnityEngine.UI;
using TMPro; // Ensure we use TMP as requested
using WordPuzzle.Game.Controllers;

namespace WordPuzzle.UI
{
    public class HUDView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button shuffleButton;
        [SerializeField] private Button submitButton;
        [SerializeField] private TextMeshProUGUI tilesLeftText; // Example of feedback
        [SerializeField] private BoardController boardController;
        [SerializeField] private RackController rackController;

        private void Awake()
        {
            ValidateAssignments();
        }

        private void OnValidate()
        {
            ValidateAssignments();
        }

        private void Start()
        {
            if (shuffleButton != null)
                shuffleButton.onClick.AddListener(OnShuffleClicked);
            
            if (submitButton != null)
                submitButton.onClick.AddListener(OnSubmitClicked);
        }

        private void OnShuffleClicked()
        {
            if (rackController != null) rackController.Shuffle();
        }

        private void OnSubmitClicked()
        {
            if (boardController != null) boardController.SubmitWord();
        }

        private void ValidateAssignments()
        {
            if (boardController == null)
            {
                Debug.LogWarning($"{nameof(HUDView)} on {name} missing BoardController reference. Assign in Inspector.", this);
            }

            if (rackController == null)
            {
                Debug.LogWarning($"{nameof(HUDView)} on {name} missing RackController reference. Assign in Inspector.", this);
            }

            if (shuffleButton == null)
            {
                Debug.LogWarning($"{nameof(HUDView)} on {name} missing Shuffle Button reference. Assign in Inspector.", this);
            }

            if (submitButton == null)
            {
                Debug.LogWarning($"{nameof(HUDView)} on {name} missing Submit Button reference. Assign in Inspector.", this);
            }

            if (tilesLeftText == null)
            {
                Debug.LogWarning($"{nameof(HUDView)} on {name} missing Tiles Left Text reference. Assign in Inspector.", this);
            }
        }
    }
}
