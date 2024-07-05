using System;
using System.Collections;
using UnityEngine;

public class HorizontalEnemyScript : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector] public float speed;
    private bool isFacingRight;
    
    [SerializeField] private GameObject deathAnim;
    private Animator animator;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (speed > 0)
        {
            isFacingRight = true;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(1, 0) * speed;
    }

    private void Update()
    {
        if (isFacingRight && speed < 0)
        {
            FlipSprite();
        }
        else if (!isFacingRight && speed > 0)
        {
            FlipSprite();
        }

        if (Mathf.Abs(speed) > 0)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    void FlipSprite()
    {
        Vector3 currentScale = transform.localScale;
        
        currentScale.x *= -1;
        
        transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }

    private void OnDestroy()
    {
        if (deathAnim != null)
        {
            var spawnedDeathAnim = Instantiate(deathAnim, transform.position, Quaternion.identity);
            if (transform.localScale.x < 0)
            {
                spawnedDeathAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}
