using System;
using UnityEngine;

public class FallThroughPlatformsScript : MonoBehaviour
{
    //References
    private Vector2 collisionPoint;
    private Transform playerFeetTransform;
    [HideInInspector] public bool canFallThrough;
    private Collider2D collider;
    private Vector2 collisionExit;

    private void Awake()
    {
        //Gets the players Feet position
        playerFeetTransform = GameObject.FindGameObjectWithTag("GroundChecker").transform;
    }

    void Start()
    {
        //Gets the collider and the highest and lowest point of the collider to determine when to turn it off or not
        collider = GetComponent<Collider2D>();
        collisionPoint = new Vector2(transform.position.x, transform.position.y + transform.lossyScale.y / 2);
        collisionExit = new Vector2(transform.position.x, transform.position.y - transform.lossyScale.y / 2);
    }
    
    void Update()
    {
        //If the player is above the platform and isn't holding down, enable collider
        if (playerFeetTransform.position.y > collisionPoint.y && !canFallThrough)
        {
            collider.enabled = true;
        }
        //else turn off collider
        else
        {
            collider.enabled = false;
        }

        
        if (playerFeetTransform.position.y < collisionExit.y)
        {
            canFallThrough = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(collisionPoint, .2f);
        Gizmos.DrawWireSphere(collisionExit, .2f);
    }
}
