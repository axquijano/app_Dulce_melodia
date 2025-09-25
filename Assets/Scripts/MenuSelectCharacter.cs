using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuSelectCharacter : MonoBehaviour
{
    private int indexCharacter = 0;
    [SerializeField] private Image imagen;
    [SerializeField] private TextMeshProUGUI nombre;
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameManager.instance;
        indexCharacter = PlayerPrefs.GetInt("CharacterSelected", 0);
        if (indexCharacter < 0 || indexCharacter >= gameManager.characters.Length)
        {
            indexCharacter = 0;
        }
        CambiarPantalla();
    }
    public void Anterior()
    {
        indexCharacter--;
        if (indexCharacter < 0)
        {
            indexCharacter = gameManager.characters.Length - 1;
        }
        CambiarPantalla();
    }

    public void Siguiente()
    {
        indexCharacter++;
        if (indexCharacter >= gameManager.characters.Length)
        {
            indexCharacter = 0;
        }
        CambiarPantalla();
    }

    private void CambiarPantalla()
    {
        PlayerPrefs.SetInt("CharacterSelected", indexCharacter);
        imagen.sprite = gameManager.characters[indexCharacter].characterSprite;
        nombre.text = gameManager.characters[indexCharacter].characterName;
    }

    public void MenuPrincipal()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
