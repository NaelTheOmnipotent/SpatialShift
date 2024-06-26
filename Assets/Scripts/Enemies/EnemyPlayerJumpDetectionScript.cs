using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerJumpDetectionScript : MonoBehaviour
{
    private InputHandlerScript playerInput;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInput = other.GetComponent<InputHandlerScript>();
        }
    }

    private void Update()
    {
        if (playerInput != null)
        {
            if (playerInput.JumpInput())
            {
                GetComponentInParent<JumpingEnemy>().playerHasJumped = true;
            }
            else
            {
                GetComponentInParent<JumpingEnemy>().playerHasJumped = false;
            }
        }
    }
}
