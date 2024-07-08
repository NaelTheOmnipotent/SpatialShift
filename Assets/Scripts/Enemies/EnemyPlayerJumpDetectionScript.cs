using UnityEngine;

public class EnemyPlayerJumpDetectionScript : MonoBehaviour
{
    //References to both the player and the InputHandler
    private InputHandlerScript playerInput;
    private GameObject playerGameObject;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //Gets the Player Input and the Player gameObject
            playerInput = other.GetComponent<InputHandlerScript>();
            playerGameObject = other.gameObject;
        }
    }

    private void Update()
    {
        if (playerInput != null)
        {
            //If the player has been assigned and the player Jumps, the Enemy will too
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
            //Rotates towards the player
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
