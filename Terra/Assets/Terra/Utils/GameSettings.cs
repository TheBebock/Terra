
using System.IO;
using Terra.Enums;
using UnityEngine;

namespace Terra.Utils
{
    public static class GameSettings
    {
        public static float DefaultGamma = 0f;
        public static float DefaultStatsOpacity = 0.3f;
        public static float DefaultItemsOpacity = 0.3f;
        public static float DefaultMasterVolume = 1.0f;
        public static float DefaultSFXVolume = 1.0f;
        public static float DefaultMusicVolume = 1.0f;
        public static bool DefaultIsFirstEverGame = true;
        public static GameDifficulty DefaultDifficultyLevel = GameDifficulty.Normal;

        private static string SettingsFilePath => Path.Combine(Application.persistentDataPath, "settings.json");

        [System.Serializable]
        private class SettingsData
        {
            public float Gamma;
            public float StatsOpacity;
            public float ItemsOpacity;
            public float MasterVolume;
            public float SFXVolume;
            public float MusicVolume;
            public bool IsFirstEverGame;
        }

        public static void TryLoadingGameSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                string json = File.ReadAllText(SettingsFilePath);
                SettingsData data = JsonUtility.FromJson<SettingsData>(json);

                DefaultGamma = data.Gamma;
                DefaultStatsOpacity = data.StatsOpacity;
                DefaultItemsOpacity = data.ItemsOpacity;
                DefaultMasterVolume = data.MasterVolume;
                DefaultSFXVolume = data.SFXVolume;
                DefaultMusicVolume = data.MusicVolume;
                DefaultIsFirstEverGame = data.IsFirstEverGame;

                Debug.Log($"Loaded game settings from {SettingsFilePath}");
            }
            else
            {
                Debug.Log($"Settings file not found, using default values.");
                Debug.Log($"Creating new settings file.");
                SaveGameSettings();
            }
        }

        public static void SaveGameSettings()
        {
            SettingsData data = new SettingsData
            {
                Gamma = DefaultGamma,
                StatsOpacity = DefaultStatsOpacity,
                ItemsOpacity = DefaultItemsOpacity,
                MasterVolume = DefaultMasterVolume,
                SFXVolume = DefaultSFXVolume,
                MusicVolume = DefaultMusicVolume,
                IsFirstEverGame = DefaultIsFirstEverGame 
            };

            string json = JsonUtility.ToJson(data, true);
            string dir = Path.GetDirectoryName(SettingsFilePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllText(SettingsFilePath, json);

            Debug.Log($"Saved game settings to {SettingsFilePath}");
        }
    }
}
