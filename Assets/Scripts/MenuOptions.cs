using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    [SerializeField]
    private GameObject[] menuPanels;
    private LevelControllerNote levelController;

    private void Start() {
        levelController = FindFirstObjectByType<LevelControllerNote>();
        levelController.OnLevelCompleted += LevelController_OnLevelCompleted;
        levelController.OnLevelFailed += LevelController_OnLevelFailed;
    }

    private void LevelController_OnLevelFailed(object sender, EventArgs e) {
        menuPanels[0].SetActive(false); // Victoria
        menuPanels[1].SetActive(true); // perder
    }

    private void LevelController_OnLevelCompleted(object sender, EventArgs e) {
        menuPanels[0].SetActive(true); // Victoria
        menuPanels[1].SetActive(false); // Perder
    }

    public void resetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void playGame() {
        SceneManager.LoadScene("Principal");
    }
}
