using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeAnimationScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private GroundedScript groundChecker;
    private Animator animator;

    
    

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("XSpeed", Mathf.Abs(rig.velocity.x));
        animator.SetFloat("YSpeed", rig.velocity.y);
        animator.SetBool("IsGrounded", groundChecker.isGrounded);
    }
}


