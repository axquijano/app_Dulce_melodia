using UnityEngine;

public class LevelLockBarrier : MonoBehaviour
{
    [Tooltip("Índice del nivel que controla esta barrera")]
    public int levelIndex;

    private SpriteRenderer sr;
    private Collider2D col;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        sr.color = Color.green; // Verde = nivel bloqueado
        col.enabled = true; // Mantiene la colisión activa
    }

    private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.IsMiniGameCompleted(levelIndex))
        {
            sr.color = new Color(1f, 1f, 1f, 0.5f); // Semitransparente = nivel completado
            col.enabled = false; // Se puede atravesar
        }


    }
}
