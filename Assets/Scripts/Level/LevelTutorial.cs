using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelTutorial : MonoBehaviour
{
    [SerializeField] private DialogueTrigger trigger;

    private bool dialogueStarted = false;
    private bool levelLoaded = false;
    private bool isSkipping = false;
    void Start()
    {
        // Inicia el diálogo solo una vez al comenzar
        if (trigger != null && !dialogueStarted)
        {
            trigger.TriggerDialogue();
            dialogueStarted = true;
        }
        else
        {
            Debug.LogWarning("DialogueTrigger no asignado en LevelTutorial");
        }
        int levels = PlayerPrefs.GetInt("LevelsUnlocked", 0);
        Debug.LogWarning($"Nivel actual: {levels}");
    }

    void Update()
    {
        if(!isSkipping){
            if (dialogueStarted && !levelLoaded && DialogueManager.dialogueManager.IsDialogFinished())
            {
                levelLoaded = true; // evita que se cargue varias veces
                StartCoroutine(LoadNextSceneAfterDelay(0.5f)); // 1 segundo de espera
            }
        }
        
    }

   IEnumerator LoadNextSceneAfterDelay(float delay)
{
    int levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", 0);
    Debug.LogWarning($"Nivel actual: {levelsUnlocked}");

    // Solo subir el contador si aún no se había desbloqueado ese nivel
    if (levelsUnlocked == 0) 
    {
        PlayerPrefs.SetInt("LevelsUnlocked", 1);
        PlayerPrefs.Save();
        Debug.LogWarning("Nuevo nivel desbloqueado: Level1");
    }

    yield return new WaitForSeconds(delay);

    string levelName = "Level" + 1;
    SceneManager.LoadScene(levelName);
}

    public void skipDialog (){
        isSkipping = true;
         int levels = PlayerPrefs.GetInt("LevelsUnlocked", 0);
        Debug.LogWarning($"Nivel actual: {levels}");
        levelLoaded = true;
        DialogueManager.dialogueManager.SkipDialog();
        StartCoroutine(LoadNextSceneAfterDelay(0.2f)); // 1 segundo de espera
    }

}
