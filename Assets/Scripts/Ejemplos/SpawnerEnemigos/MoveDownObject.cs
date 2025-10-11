using UnityEngine;

public class MoveDownObject : MonoBehaviour
{
    public float speed = 1f; // Speed at which the object moves down
   

    void Start()
    {
        Destroy(gameObject, 5f); // Destroy the object after 5 seconds to prevent memory leaks
    }

    void Update()
    {
        transform.position += -transform.up * speed * Time.deltaTime; // Move the object downwards
    }
}
