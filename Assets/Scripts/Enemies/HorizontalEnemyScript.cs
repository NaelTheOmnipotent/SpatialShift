using UnityEngine;

public class HorizontalEnemyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector] public float speed;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(1, 0) * speed;
    }
}
