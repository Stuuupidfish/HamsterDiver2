using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class PlayerData 
{
    private const int LevelCount = 7;
    private static bool[] levelsUnlocked = new bool[LevelCount] { true, false, false, false, false, false, false };
    private static int[] levelScores = new int[LevelCount] { 0, 0, 0, 0, 0, 0, 0 };
    private static float endlessHighScore = 0f;

    public static event Action OnChanged;

    public static bool IsLevelUnlocked(int levelIndex)
    {
        return IsValidLevel(levelIndex) && levelsUnlocked[levelIndex];
    }

    public static int GetLevelScore(int levelIndex)
    {
        return IsValidLevel(levelIndex) ? levelScores[levelIndex] : 0;
    }

    public static float GetEndlessHighScore()
    {
        return endlessHighScore;
    }

    public static void UnlockLevel(int levelIndex)
    {
        if (!IsValidLevel(levelIndex) || levelsUnlocked[levelIndex])
        {
            return;
        }

        levelsUnlocked[levelIndex] = true;
        OnChanged?.Invoke();
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
            OnChanged?.Invoke();
        }
    }

    public static void SetEndlessHighScore(float distance)
    {
        if (distance <= endlessHighScore)
        {
            return;
        }

        endlessHighScore = distance;
        OnChanged?.Invoke();
    }

    private static bool IsValidLevel(int levelIndex)
    {
        return levelIndex >= 0 && levelIndex < LevelCount;
    }
    
}
