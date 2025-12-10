using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WordPuzzle.Core;
using WordPuzzle.Game.Model;

namespace WordPuzzle.UI
{
    /// <summary>
    /// Displays end-of-game state without obscuring the board.
    /// </summary>
    public class EndGamePanelController : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject panelRoot;

        [Header("Text Elements")]
        [SerializeField] private TextMeshProUGUI emojiText;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI subtitleText;
        [SerializeField] private TextMeshProUGUI recapText;

        [Header("Buttons")]
        [SerializeField] private Button replayButton;
        [SerializeField] private Button nextButton;

        private void Awake()
        {
            if (replayButton != null) replayButton.onClick.AddListener(OnReplay);
            if (nextButton != null) nextButton.onClick.AddListener(OnNext);
        }

        private void OnEnable()
        {
            EventManager.StartListening("EndGame", OnEndGame);
        }

        private void OnDisable()
        {
            EventManager.StopListening("EndGame", OnEndGame);
        }

        private void OnEndGame(object payload)
        {
            var data = payload as EndGamePayload;
            if (data == null) return;

            if (panelRoot != null) panelRoot.SetActive(true);

            ApplyVisuals(data);
            ApplyRecap(data.wordsPlayed);
        }

        private void ApplyVisuals(EndGamePayload data)
        {
            string emoji = data.result == EndGameResult.Win ? "🎉" : "🤔";
            if (emojiText != null) emojiText.text = emoji;

            if (titleText != null)
            {
                if (data.result == EndGameResult.Win)
                {
                    titleText.text = GetWinTitle(data.tilesRemaining);
                }
                else
                {
                    titleText.text = "Nice Try!";
                }
            }

            if (subtitleText != null)
            {
                if (data.result == EndGameResult.Lose)
                {
                    subtitleText.text = "No more valid moves available.";
                }
                else
                {
                    subtitleText.text = $"Solutions: {data.totalSolutions} | Paths: {data.totalPaths}";
                }
            }
        }

        private void ApplyRecap(List<string> wordsPlayed)
        {
            if (recapText == null || wordsPlayed == null) return;

            var sb = new StringBuilder();
            for (int i = 0; i < wordsPlayed.Count; i++)
            {
                bool isFinal = i == wordsPlayed.Count - 1;
                var word = wordsPlayed[i];
                if (isFinal)
                {
                    sb.AppendLine($"{i + 1}. <color=#FFB020>{word}</color>");
                }
                else
                {
                    sb.AppendLine($"{i + 1}. {word}");
                }
            }

            recapText.text = sb.ToString();
        }

        private string GetWinTitle(int tilesRemaining)
        {
            if (tilesRemaining <= 0) return "Perfect!";
            if (tilesRemaining == 1) return "Amazing!";
            if (tilesRemaining == 2) return "Great Job!";
            if (tilesRemaining == 3) return "Good Effort!";
            return "Nice Try!";
        }

        private void OnReplay()
        {
            if (panelRoot != null) panelRoot.SetActive(false);
            GameManager.Instance.StartGame();
        }

        private void OnNext()
        {
            if (panelRoot != null) panelRoot.SetActive(false);
            GameManager.Instance.NextLevel();
        }
    }
}
