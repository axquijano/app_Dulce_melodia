using UnityEngine;

public class MiraMovement : MonoBehaviour
{
    private Vector2 targetPosition;

    void Update()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = targetPosition;
    }
}
