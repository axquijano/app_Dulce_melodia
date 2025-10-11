using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El objetivo que la cámara seguirá
    public Vector2 positionMinima;
    public Vector2 positionMaxima;  

    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(target.position.x, positionMinima.x, positionMaxima.x), Mathf.Clamp(target.position.y, positionMinima.y, positionMaxima.y), transform.position.z);
    }
}
