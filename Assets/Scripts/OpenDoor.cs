using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public TMP_Text text;
    public string levelName;
    private bool inDoor = false;
    private Transform playerTransform; // referencia al jugador


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
            inDoor = true;
            playerTransform = collision.transform; // guardamos al jugador
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (text != null && text.gameObject != null)
        {
            text.gameObject.SetActive(false);
        }
        inDoor = false;
        playerTransform = null; // soltamos la referencia
    }

    private void Update()
    {
        if (inDoor && Input.GetKeyDown(KeyCode.E))
        {
            if (text != null) text.gameObject.SetActive(false);
            // Guardar checkpoint con la posición actual del jugador
            if (playerTransform != null)
            {
                GameManager.instance.SaveCheckpoint(playerTransform.position);
            }
            // Cargar el minijuego
            SceneManager.LoadScene(levelName);
        }
    }
}
