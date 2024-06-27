using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerJumpDetectionScript : MonoBehaviour
{
    private InputHandlerScript playerInput;
    private GameObject playerGameObject;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInput = other.GetComponent<InputHandlerScript>();
            playerGameObject = other.gameObject;
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

        if (playerGameObject != null)
        {
            if (playerGameObject.transform.position.x > transform.position.x)
            {
                GetComponentInParent<JumpingEnemy>().FacingRight();
            }
            else if (playerGameObject.transform.position.x < transform.position.x)
            {
                GetComponentInParent<JumpingEnemy>().FacingLeft();
            }
        }
    }
}
