using System;
using UnityEngine;

public static class PlayerData 
{
    private const int LevelCount = 7;
    private static bool[] levelsUnlocked = new bool[LevelCount] { true, false, false, false, false, false, false };
    private static bool[] levelsBeaten = new bool[LevelCount] { false, false, false, false, false, false, false };
    private static int[] levelScores = new int[LevelCount] { 0, 0, 0, 0, 0, 0, 0 };
    private static float endlessHighScore = 0f;
    public static float EndlessHighScore
    {
        get { return endlessHighScore; }
    }
    public static event Action OnChanged;

    [Serializable]
    public class SaveData
    {
        public bool[] levelsUnlocked;
        public bool[] levelsBeaten;
        public int[] levelScores;
        public float endlessHighScore;
    }

    public static SaveData GetSaveData()
    {
        SaveData saveData = new SaveData();
        saveData.levelsUnlocked = (bool[])levelsUnlocked.Clone();
        saveData.levelsBeaten = (bool[])levelsBeaten.Clone();
        saveData.levelScores = (int[])levelScores.Clone();
        saveData.endlessHighScore = endlessHighScore;
        return saveData;
    }

    public static void LoadSaveData(SaveData saveData)
    {
        if (saveData == null)
        {
            return;
        }

        if (saveData.levelsUnlocked != null && saveData.levelsUnlocked.Length == LevelCount)
        {
            levelsUnlocked = (bool[])saveData.levelsUnlocked.Clone();
        }

        if (saveData.levelsBeaten != null && saveData.levelsBeaten.Length == LevelCount)
        {
            levelsBeaten = (bool[])saveData.levelsBeaten.Clone();
        }

        if (saveData.levelScores != null && saveData.levelScores.Length == LevelCount)
        {
            levelScores = (int[])saveData.levelScores.Clone();
        }

        endlessHighScore = saveData.endlessHighScore;
    }

    public static bool IsLevelUnlocked(int levelIndex)
    {
        return IsValidLevel(levelIndex) && levelsUnlocked[levelIndex];
    }

    public static int GetLevelScore(int levelIndex)
    {
        if (!IsValidLevel(levelIndex))
        {
            return 0;
        }

        return levelScores[levelIndex];
    }

    public static void UnlockLevel(int levelIndex)
    {
        if (!IsValidLevel(levelIndex) || levelsUnlocked[levelIndex])
        {
            return;
        }

        levelsUnlocked[levelIndex] = true;
        if (OnChanged != null)
        {
            OnChanged.Invoke();
        }
    }
    public static void MarkLevelBeaten(int levelIndex)
    {
        if (!IsValidLevel(levelIndex) || levelsBeaten[levelIndex])
        {
            return;
        }

        levelsBeaten[levelIndex] = true;
        if (OnChanged != null)
        {
            OnChanged.Invoke();
        }
    }
    public static bool IsLevelBeaten(int levelIndex)
    {
        return IsValidLevel(levelIndex) && levelsBeaten[levelIndex];
    }
    public static void SetLevelScore(int levelIndex, int score)
    {
        if (!IsValidLevel(levelIndex))
        {
            return;
        }

        if (score > levelScores[levelIndex])
        {
            levelScores[levelIndex] = score;
            if (OnChanged != null)
            {
                OnChanged.Invoke();
            }
        }
    }

    public static void SetEndlessHighScore(float distance)
    {
        if (distance <= endlessHighScore)
        {
            return;
        }

        endlessHighScore = distance;
        if (OnChanged != null)
        {
            OnChanged.Invoke();
        }
    }

    private static bool IsValidLevel(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < LevelCount;
    }
    
}
