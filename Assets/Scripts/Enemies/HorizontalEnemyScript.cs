using System;
using System.Collections;
using UnityEngine;

public class HorizontalEnemyScript : MonoBehaviour
{
    //References
    private Rigidbody2D rb;
    [HideInInspector] public float speed;
    private bool isFacingRight;
    
    [SerializeField] private GameObject deathAnim;
    private Animator animator;
    
    private void Awake()
    {
        //Gets the Components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        //Makes sure the enemy is facing the right way
        if (speed > 0)
        {
            isFacingRight = true;
        }
    }

    private void FixedUpdate()
    {
        //Move
        rb.velocity = new Vector2(1, 0) * speed;
    }

    private void Update()
    {
        //Turn Around
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
            //IsMoving
            animator.SetBool("IsMoving", true);
        }
        else
        {
            //Isn#t moving
            animator.SetBool("IsMoving", false);
        }
    }

    void FlipSprite()
    {
        //Scales the x transform * -1
        Vector3 currentScale = transform.localScale;
        
        currentScale.x *= -1;
        
        transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }

    private void OnDestroy()
    {
        //Spawns a death animation when it dies
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
