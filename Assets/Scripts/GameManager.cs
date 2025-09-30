using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Character[] characters; // Array to hold character ScriptableObjects

    public Vector3 lastCheckpointPosition;
    public bool hasCheckpoint = false;
    public bool[] levelsCompleted = new bool[3];// Assuming 3 levels for example

    private void Awake()
    {
        // Implement singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        hasCheckpoint = true;
    }

    public void CompleteLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelsCompleted.Length)
        {
            levelsCompleted[levelIndex] = true;
        }
    }

    public bool IsLevelCompleted(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levelsCompleted.Length)
        {
            return levelsCompleted[levelIndex];
        }
        return false;
    }

}
