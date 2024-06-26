using System;
using UnityEngine;

public class FallThroughPlatformsScript : MonoBehaviour
{
    private Vector2 collisionPoint;
    private Transform playerFeetTransform;
    [HideInInspector] public bool canFallThrough;
    private Collider2D collider;
    private Vector2 collisionExit;

    private void Awake()
    {
        playerFeetTransform = GameObject.FindGameObjectWithTag("GroundChecker").transform;
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        collisionPoint = new Vector2(transform.position.x, transform.position.y + transform.lossyScale.y / 2);
        collisionExit = new Vector2(transform.position.x, transform.position.y - transform.lossyScale.y / 2);
    }
    
    void Update()
    {
        if (playerFeetTransform.position.y > collisionPoint.y && !canFallThrough)
        {
            collider.enabled = true;
        }
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
