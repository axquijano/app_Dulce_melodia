using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    

    public void StartGame()
    {   
        int levels = PlayerPrefs.GetInt("LevelsUnlocked", 0);
        Debug.LogWarning($"Nivel actual Main menu: {levels}");
        String levelName = "Level" + levels;
        // Cargar la escena del nivel
        SceneManager.LoadScene(levelName);
        
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit(); /* Este va funcionar cuando se exporte el juego  */
    }
    
    public void SelectCharacterMenu()
    {
        int levels = PlayerPrefs.GetInt("LevelsUnlocked", 0);
        Debug.LogWarning($"Nivel actual Main menu: {levels}");
        SceneManager.LoadScene("CharacterSelect");
    }
    
}
