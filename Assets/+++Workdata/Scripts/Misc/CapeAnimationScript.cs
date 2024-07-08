using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeAnimationScript : MonoBehaviour
{
    //References
    [SerializeField] private Rigidbody2D rig;
    [SerializeField] private GroundedScript groundChecker;
    [SerializeField] private GameObject cape;
    [SerializeField] private float rayCastDistance;
    private SpriteRendererScript spriteRendererScript;
    private Animator animator;
    private bool isGrounded;

    private Vector3 startingPos;
    [SerializeField] private LayerMask groundMask;

    private void Start()
    {
        //spawns the cape animation
        animator = GetComponent<Animator>();
        startingPos = transform.position;
        spriteRendererScript = GetComponentInParent<SpriteRendererScript>();
    }

    private void Update()
    {
        //sets variables for the animator
        animator.SetFloat("XSpeed", Mathf.Abs(rig.velocity.x));
        animator.SetFloat("YSpeed", rig.velocity.y);
        animator.SetBool("IsGrounded", isGrounded);
        
        CapePosition();
    }

    private void FixedUpdate()
    {
        //seperate ground check for the cape animation
        RaycastHit2D leftGroundHit = Physics2D.Raycast(rig.transform.position + new Vector3(-0.25f, 0, 0), -Vector2.up, rayCastDistance, groundMask);
        RaycastHit2D rightGroundHit = Physics2D.Raycast(rig.transform.position + new Vector3(0.25f, 0, 0), -Vector2.up, rayCastDistance, groundMask);

        if (leftGroundHit.collider || rightGroundHit.collider)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void CapePosition()
    {
        //changes the capes position in relation to the player depending on the players current state
        if (rig.velocity.y <= -0.1 && !isGrounded)
        {
            cape.transform.localPosition = new Vector3(-0.29f, 0.12f, 0f);

        }
        else
        {
            cape.transform.localPosition = new Vector3(-0.18f, 0f, 0f);
        }

        if (spriteRendererScript.isBraking)
        {
            cape.transform.localPosition = new Vector3(-0.02f, 0f, 0f);
        }

        if (rig.velocity.y >= 0.1 && rig.velocity.x >= 0.1 && !isGrounded )
        {
            cape.transform.localPosition = new Vector3(-0.29f, 0.12f, 0f);
        }
    }
}


