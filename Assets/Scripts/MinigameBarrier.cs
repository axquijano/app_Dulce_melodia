using UnityEngine;
using System;

public class MinigameBarrier : MonoBehaviour
{
    public int levelIndex;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (GameManager.instance != null && GameManager.instance.IsLevelCompleted(levelIndex))
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f); // transparente si ya completado
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            sr.color = Color.green; // verde desde el inicio si no está completado
            GetComponent<Collider2D>().enabled = true;
        }
    }

    private void Update()
    {
        // Si quieres, puedes mantener esto por seguridad si cambia en tiempo real
        if (GameManager.instance != null && GameManager.instance.IsLevelCompleted(levelIndex))
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

