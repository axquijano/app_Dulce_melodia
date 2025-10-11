using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public Character[] characters; // Array to hold character ScriptableObjects

    public Vector3 lastCheckpointPosition;
    public bool hasCheckpoint = false;
    public bool[] IsMiniGameComplete = new bool[3];// Assuming 3 levels for example

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

    public void CompleteMiniGame(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < IsMiniGameComplete.Length)
        {
            IsMiniGameComplete[levelIndex] = true;
        }
    }

    public bool IsMiniGameCompleted(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < IsMiniGameComplete.Length)
        {
            return IsMiniGameComplete[levelIndex];
        }
        return false;
    }
    
}
