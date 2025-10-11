using UnityEngine;

public class DamageObjectMiniGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Player>().PlayerDamaged();
        }
    }
}
