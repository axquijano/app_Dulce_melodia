using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Character[] characters; // Array to hold character ScriptableObjects

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


}
