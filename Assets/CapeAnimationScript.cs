using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeAnimationScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private GroundedScript groundChecker;
    [SerializeField] private GameObject cape;
    private Animator animator;

    private Vector3 startingPos;
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        startingPos = transform.position;
    }

    private void Update()
    {
        animator.SetFloat("XSpeed", Mathf.Abs(rig.velocity.x));
        animator.SetFloat("YSpeed", rig.velocity.y);
        animator.SetBool("IsGrounded", groundChecker.isGrounded);
        
        CapePosition();
    }

    private void CapePosition()
    {
        if (rig.velocity.y <= -0.1)
        {
            cape.transform.localPosition = new Vector3(-0.29f, 0.12f, 0f);

        }
        else
        {
            cape.transform.localPosition = new Vector3(-0.18f, 0f, 0f);
        }
        
    }
}


