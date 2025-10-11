using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class FindTheNoteManager : MonoBehaviour, IMinigameManager
{
    [Serializable]
    public class LevelSettings
    {
        public int columns;       // número de columnas en la grilla de cartas
        public int rows;          // número de filas en la grilla de cartas
        public int maxAttempts;   // intentos máximos permitidos (para feedback en el futuro)
        public int cardType = -1; // tipo fijo de carta para este nivel (-1 = aleatorio)
    }

    [Header("UI")]
    [SerializeField] private TMP_Text levelLabel;
    [SerializeField] private TMP_Text feedbackLabel;
    [SerializeField] private TMP_Text timerLabel;

    [Header("Prefabs")]
    [SerializeField] private NoteCardController cardPrefab;       // prefab de carta genérica
    [SerializeField] private NoteCardController mainCardPrefab;   // carta principal (pictograma objetivo)

    [Header("Levels")]
    [SerializeField] private List<LevelSettings> levels = new List<LevelSettings>();

    private List<NoteCardController> spawnedCards = new List<NoteCardController>();
    private int currentLevel = 0;
    private float elapsedTime = 0f;
    private int targetCardType = -1; // tipo de carta que el jugador debe encontrar

    public event EventHandler OnLevelCompleted;
    public event EventHandler OnLevelFailed;

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("Level", 0);
        Debug.Log($"Nivel actual: {currentLevel}");
        SetupLevel();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        timerLabel.text = $"Tiempo: {elapsedTime:F1}s";
    }

    private void SetupLevel()
    {
        ClearCards();

        LevelSettings settings = levels[currentLevel];

        // Determinar el tipo de carta objetivo
        if (settings.cardType >= 0 && settings.cardType < cardPrefab.MaxCardTypes)
            targetCardType = settings.cardType; // usar tipo definido en el nivel
        else
            targetCardType = UnityEngine.Random.Range(0, cardPrefab.MaxCardTypes); // elegir aleatorio

        // Inicializar carta principal (la que el jugador debe adivinar)
        mainCardPrefab.InitCardType(targetCardType);
        Debug.Log($"Carta principal:(tipo {targetCardType})");

        // Lista de tipos disponibles para poblar la grilla
        List<int> availableTypes = new List<int>();
        for (int i = 0; i < cardPrefab.MaxCardTypes; i++) {
            availableTypes.Add(i);
        }

        //Se seleccionan los tipos de cartas que se van a usar en este nivel
        List<int> gameTypes = new List<int>();
        for (int i = 0; i < settings.columns * settings.rows; i++)
        {
            int chosenType = availableTypes[UnityEngine.Random.Range(0, availableTypes.Count)];
            availableTypes.Remove(chosenType);
            gameTypes.Add(chosenType);
        }

        // Asegurar que el tipo objetivo esté incluido en las cartas de juego
        if (!gameTypes.Contains(targetCardType)) {
            gameTypes[UnityEngine.Random.Range(0, gameTypes.Count)] = targetCardType;
        }

        // Spawnear cartas en una grilla
        for (int row = 0; row < settings.rows; row++)
        {
            for (int col = 0; col < settings.columns; col++)
            {
                Vector3 offset = new Vector3((settings.columns - 2f) * cardPrefab.cardSize,
                                             (settings.rows - 2.2f) * cardPrefab.cardSize, 0) * 0.5f;

                Vector3 position = new Vector3(col * cardPrefab.cardSize, row * cardPrefab.cardSize, 0);

                int type = gameTypes[UnityEngine.Random.Range(0, gameTypes.Count)];
                NoteCardController card = Instantiate(cardPrefab, position - offset, Quaternion.identity);

                card.InitCardType(type, true);
                card.onClicked.AddListener(OnCardSelected);
                gameTypes.Remove(type);
                spawnedCards.Add(card);
            }
        }

        levelLabel.text = $"Nivel {currentLevel + 1}";
        feedbackLabel.text = "Escoge la carta que coincide con la principal";
    }

    private void OnCardSelected(NoteCardController selectedCard)
    {
        if (selectedCard.cardType == targetCardType)
        {
            feedbackLabel.text = "¡Correcto!";
            OnLevelCompleted?.Invoke(this, EventArgs.Empty);
            GameManager.instance.CompleteMiniGame(0);
            AdvanceLevel();
        }
        else
        {
            feedbackLabel.text = "Intenta de nuevo";
            OnLevelFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void AdvanceLevel()
    {
        currentLevel++;
        if (currentLevel >= levels.Count) currentLevel = 0;

        PlayerPrefs.SetInt("Level", currentLevel);
        PlayerPrefs.Save();

    }

    private void ResetLevel()
    {
        SetupLevel();
    }

    private void ClearCards()
    {
        foreach (var card in spawnedCards) Destroy(card.gameObject);
        spawnedCards.Clear();
    }
}
