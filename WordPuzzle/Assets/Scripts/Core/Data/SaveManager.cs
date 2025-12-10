using System;
using System.IO;
using UnityEngine;

namespace WordPuzzle.Core.Data
{
    [Serializable]
    public class GameData
    {
        public int currentLevelIndex;
        public int currentRackSize;
        // Optimization: Save puzzle seeds or IDs to restore exact state?
        // For now: Just progress tracking.
        
        public GameData()
        {
            currentLevelIndex = 1;
            currentRackSize = 5;
        }
    }

    public static class SaveManager
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "gamedata.json");

        public static GameData Load()
        {
            if (File.Exists(SavePath))
            {
                try
                {
                    string json = File.ReadAllText(SavePath);
                    return JsonUtility.FromJson<GameData>(json);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to load save data: " + e.Message);
                }
            }
            return new GameData(); // Default
        }

        public static void Save(GameData data)
        {
            try
            {
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(SavePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save data: " + e.Message);
            }
        }
    }
}
