using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigamePortal : MonoBehaviour
{
    [Tooltip("Texto que se muestra al jugador cuando está cerca de la puerta")]
    public TMP_Text promptText;

    [Tooltip("Nombre de la escena/minijuego que se cargará")]
    public string levelName;

    private bool isPlayerNearby = false;
    private Transform playerTransform; // referencia al jugador

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (promptText != null) promptText.gameObject.SetActive(true); // mostrar texto
            isPlayerNearby = true;
            playerTransform = collision.transform; // guardar referencia al jugador
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (promptText != null) promptText.gameObject.SetActive(false); // ocultar texto
        isPlayerNearby = false;
        playerTransform = null; // limpiar referencia
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (promptText != null) promptText.gameObject.SetActive(false);

            // Guardar la posición del jugador como checkpoint
            if (playerTransform != null)
            {
                GameManager.instance.SaveCheckpoint(playerTransform.position);
            }

            // Cargar la escena del minijuego
            SceneManager.LoadScene(levelName);
        }
    }
}
