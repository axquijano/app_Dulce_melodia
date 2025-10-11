using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Variable para almacenar el nivel
    public int levels = 1;
    
    public void StartGame()
    {   
        levels = PlayerPrefs.GetInt("LevelsUnlocked", levels);
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
        SceneManager.LoadScene("CharacterSelect");
    }
}
