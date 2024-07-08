using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayingPlatformScripts : MonoBehaviour
{
    [HideInInspector] public float decayTime = 3;
    private Vector2 topOfPlatform;

    private void Start()
    {
        //Calculates the topmost point of the platform
        topOfPlatform = new Vector2(transform.position.x, transform.position.y + transform.lossyScale.y / 2);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //If the player is above that but is touching the platform it dissapears after time
            var playerFeetPos = new Vector2(other.transform.position.x, other.transform.position.y - other.transform.lossyScale.y / 2);
            if (playerFeetPos.y >= topOfPlatform.y)
            {
                Destroy(gameObject, decayTime);
            }
        }
    }
}
