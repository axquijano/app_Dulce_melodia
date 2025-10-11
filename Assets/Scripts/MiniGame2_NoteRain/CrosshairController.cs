using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private Vector2 targetPosition;

    void Update()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = targetPosition;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D collider = Physics2D.OverlapPoint(targetPosition);
            NoteSphereController sphere = collider?.GetComponent<NoteSphereController>();

            if (collider != null && collider.CompareTag("Spheres"))
            {
                sphere?.SphereAnimate();
                Debug.Log($"Sphere name {sphere.prefabsSphere[sphere.sphereType].noteName} clicked");
                Destroy(collider.gameObject, 1f); // Delay to allow animation to pla
            }
        }
    }
}
