using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelectionMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image imagen;
    [SerializeField] private TextMeshProUGUI characterName;
    private GameManager gameManager;
    private int indexCharacter = 0;
    void Start()
    {
        gameManager = GameManager.instance;
        indexCharacter = PlayerPrefs.GetInt("CharacterSelected", 0);
        if (indexCharacter < 0 || indexCharacter >= gameManager.characters.Length)
        {
            indexCharacter = 0;
        }
        UpdateCharacterDisplay();
    }

    // UI botón anterior personaje
    public void PreviousCharacter()
    {
        indexCharacter--;
        if (indexCharacter < 0)
        {
            indexCharacter = gameManager.characters.Length - 1;
        }
        UpdateCharacterDisplay();
    }

    // UI botón siguiente personaje
    public void NextCharacter()
    {
        indexCharacter++;
        if (indexCharacter >= gameManager.characters.Length)
        {
            indexCharacter = 0;
        }
        UpdateCharacterDisplay();
    }

    // Actualiza la imagen y el nombre del personaje seleccionado
    private void UpdateCharacterDisplay()
    {
        PlayerPrefs.SetInt("CharacterSelected", indexCharacter);
        imagen.sprite = gameManager.characters[indexCharacter].characterSprite;
        characterName.text = gameManager.characters[indexCharacter].characterName;
    }

    // UI botón volver al menú principal
    public void BackToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
