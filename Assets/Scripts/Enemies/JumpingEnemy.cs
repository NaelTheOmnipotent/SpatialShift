using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    [HideInInspector] public bool playerHasJumped;
    [HideInInspector] public float jumpForce;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        if (playerHasJumped && rb.velocity.y == 0) 
        { 
            rb.velocity = Vector2.up * jumpForce;
        }
    }
}
