using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddPuntuacion : MonoBehaviour
{
    public float score;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            FindFirstObjectByType<MiniGameManager>().AddPuntuacion(score);
        }
    }
}
