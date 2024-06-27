using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [HideInInspector] public bool playerHasJumped;
    [HideInInspector] public float jumpForce;

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
}
