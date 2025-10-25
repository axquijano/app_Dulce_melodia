using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private string nombreManager = "NoteRainManager";
    private IMinigameManager minigameManager;

    private void Start()
    {
        // Obtener el tipo por nombre
        Type managerType = Type.GetType(nombreManager);

        if (managerType == null)
        {
            Debug.LogError($"❌ No se encontró el tipo con nombre {nombreManager}");
            return;
        }

        // Buscar el objeto que tenga ese componente
        MonoBehaviour managerInstance = FindFirstObjectByType(managerType) as MonoBehaviour;

        if (managerInstance == null)
        {
            Debug.LogError($"❌ No se encontró ningún objeto con el componente {nombreManager}");
            return;
        }

        // Intentar convertirlo a la interfaz
        minigameManager = managerInstance as IMinigameManager;

        if (minigameManager == null)
        {
            Debug.LogError($"❌ El componente {nombreManager} no implementa IMinigameManager");
            return;
        }

        // Suscribirse a los eventos
        minigameManager.OnLevelCompleted += OnLevelCompleted;
        minigameManager.OnLevelFailed += OnLevelFailed;
    }

    private void OnDestroy()
    {
        if (minigameManager != null)
        {
            minigameManager.OnLevelCompleted -= OnLevelCompleted;
            minigameManager.OnLevelFailed -= OnLevelFailed;
        }
    }

    private void OnLevelCompleted(object sender, EventArgs e)
    {
        //Pausar el juego
        Time.timeScale = 0;
        winPanel.SetActive(true);
        losePanel.SetActive(false);
    }

    private void OnLevelFailed(object sender, EventArgs e)
    {
        Time.timeScale = 0;
        winPanel.SetActive(false);
        losePanel.SetActive(true);
    }

    public void ResetGame()
    {
        // Reiniciar el juego
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayGame(string sceneName = "Level1")
    {
        Time.timeScale = 1;
        int inlevel = PlayerPrefs.GetInt("LevelsUnlocked", 0);
        Debug.LogWarning($"GameUI Nivel actual: {inlevel}");
        string levelName = "Level" + inlevel;
        SceneManager.LoadScene(levelName);
    }
}
