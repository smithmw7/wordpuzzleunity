using TMPro;
using UnityEngine;
using WordPuzzle.Core;
using WordPuzzle.Game.Model;

/// <summary>
/// Displays puzzle debug metadata (solutions/paths) in a bound TMP text field.
/// </summary>
public class PuzzleDebugPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    private void Awake()
    {
        if (debugText == null)
        {
            Debug.LogWarning($"{nameof(PuzzleDebugPanel)} on {name} is missing a debug Text reference.", this);
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening("LevelLoaded", OnLevelLoaded);
    }

    private void OnDisable()
    {
        EventManager.StopListening("LevelLoaded", OnLevelLoaded);
    }

    private void OnLevelLoaded(object payload)
    {
        if (debugText == null) return;

        var level = payload as LevelData;
        if (level == null) return;

        debugText.text = $"total_solutions: {level.totalSolutions}\n" +
                         $"total_paths: {level.totalPaths}";
    }
}
