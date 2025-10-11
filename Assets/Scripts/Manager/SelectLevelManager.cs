using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SelectLevelManager : MonoBehaviour
{

    [Header("Buttons")]
    public List<Button> levelButtons = new List<Button>();
    public int levelsUnlocked = 1;

    public void Start()
    {
        UpdateLevelButtons();
    }   

    public void changeLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void changeLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    public void UpdateLevelButtons()
    {
        levelsUnlocked = PlayerPrefs.GetInt("LevelsUnlocked", levelsUnlocked);

        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i < levelsUnlocked)
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }

    public void UnlockNextLevel()
    {
        levelsUnlocked++;
        PlayerPrefs.SetInt("LevelsUnlocked", levelsUnlocked);
        UpdateLevelButtons();
    }
}
