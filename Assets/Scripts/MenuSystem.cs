using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit(); /* Este va funcionar cuando se exporte el juego  */
    }
    
    public void SelectCharacterMenu()
    {
        SceneManager.LoadScene("SelectCharacterMenu");
    }
}
