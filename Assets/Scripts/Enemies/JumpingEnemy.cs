using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [HideInInspector] public bool playerHasJumped;
    [HideInInspector] public float jumpForce;

    [SerializeField] private GameObject jumpingDeathAnim;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        if (playerHasJumped && rb.velocity.y == 0) 
        { 
            rb.velocity = Vector2.up * jumpForce;
        }

        if (rb.velocity.y != 0)
        {
            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }
        animator.SetFloat("YSpeed", rb.velocity.y);
    }

    public void FacingRight()
    {
        Vector3 currentScale = transform.localScale;

        currentScale.x = Mathf.Abs(currentScale.x);

        transform.localScale = currentScale;
    }

    public void FacingLeft()
    {
        Vector3 currentScale = transform.localScale;

        currentScale.x = -Mathf.Abs(currentScale.x);

        transform.localScale = currentScale;
    }

    private void OnDestroy()
    {
        if (jumpingDeathAnim != null)
        {
            var spawnedDeathAnim = Instantiate(jumpingDeathAnim, transform.position, Quaternion.identity);
            if (transform.localScale.x > 0)
            {
                spawnedDeathAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}
