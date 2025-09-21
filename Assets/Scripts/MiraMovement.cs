using UnityEngine;

public class MiraMovement : MonoBehaviour
{
    /* [SerializeField] 
    private float moveSpeed = 5f; */
    
    private Vector2 targetPosition;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        /* este codigo es con delay */
        /* transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); */
        transform.position = targetPosition;
    }
}
