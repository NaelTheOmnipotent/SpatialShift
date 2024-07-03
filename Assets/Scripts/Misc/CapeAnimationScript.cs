using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapeAnimationScript : MonoBehaviour
{
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
        animator = GetComponent<Animator>();
        startingPos = transform.position;
        spriteRendererScript = GetComponentInParent<SpriteRendererScript>();
    }

    private void Update()
    {
        animator.SetFloat("XSpeed", Mathf.Abs(rig.velocity.x));
        animator.SetFloat("YSpeed", rig.velocity.y);
        animator.SetBool("IsGrounded", isGrounded);
        
        CapePosition();
    }

    private void FixedUpdate()
    {
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rig.transform.position + new Vector3(-0.25f, 0, 0), rig.transform.position + new Vector3(-0.25f, -rayCastDistance, 0));
        
    }
}


