using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NoteRainManager : MonoBehaviour, IMinigameManager
{
    [Serializable]
    public class LevelSettings
    {
        [Header("Spawner Settings")]
        public string[] targetNoteNames; // notas objetivo
        public float spawnIntervalTargetNotes = 3f;
        public float initialDelayTargetNotes = 1f;
        public float spawnIntervalObstacleNotes = 3f;
        public float initialDelayObstacleNotes = 1f;
    }

    [Header("Text UI")]
    public TMP_Text hitsText;
    public TMP_Text missesText;
    public TMP_Text timeText;
    public NoteCardController targetNoteCard;

    [Header("Game Settings")]
    public int hitsToWin = 5;
    public int missesToLose = 3;

    [Header("Level Settings")]
    [SerializeField] private List<LevelSettings> levels = new List<LevelSettings>();

    [Header("References")]
    [SerializeField] private SphereSpawner sphereSpawner;

    private float hits = 0;
    private float misses = 0;
    private float time = 0;
    private int currentLevel = 0;

    public event EventHandler OnLevelCompleted;
    public event EventHandler OnLevelFailed;

    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("Level", 0);
        Debug.Log($"Nivel actual: {currentLevel}");

        LevelSettings settings = levels[currentLevel];
        string targetNameNote = settings.targetNoteNames[0];
        int targetType = targetNoteCard.getTypeForName(targetNameNote);
        targetNoteCard.InitCardType(targetType, true);

        // 💡 Configurar el spawner según el nivel
        sphereSpawner.ConfigureSpawner(
            settings.targetNoteNames,
            settings.spawnIntervalTargetNotes,
            settings.initialDelayTargetNotes,
            settings.spawnIntervalObstacleNotes,
            settings.initialDelayObstacleNotes
        );

        Time.timeScale = 1;
    }
    
    void Update()
    {
        time += Time.deltaTime;
        timeText.text = time.ToString("0.00");
    }

     public void AddHit()
    {
        hits++;
        hitsText.text = hits.ToString();

        if (hits >= hitsToWin)
        {
            Time.timeScale = 0;
            Debug.Log("🎉 Ganaste!");
            GameManager.instance.CompleteMiniGame(1); // marcar nivel 1 como completado
            OnLevelCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    public void AddMiss()
    {
        misses++;
        missesText.text = misses.ToString();

        if (misses >= missesToLose)
        {
            Debug.Log("💀 Perdiste!");
            Time.timeScale = 0;
            OnLevelFailed?.Invoke(this, EventArgs.Empty);
        }
    }
}
