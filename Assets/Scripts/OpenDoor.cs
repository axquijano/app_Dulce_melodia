using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenDoor : MonoBehaviour
{
    public TMP_Text text;
    public string levelName;
    private bool inDoor = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);
            inDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (text != null && text.gameObject != null)
        {
            text.gameObject.SetActive(false);
        }
        inDoor = false;
    }

    private void Update()
    {
        if (inDoor && Input.GetKeyDown(KeyCode.E))
        {
            if (text != null) text.gameObject.SetActive(false);
            // Load the new level
            SceneManager.LoadScene(levelName);
        }
    }
}
