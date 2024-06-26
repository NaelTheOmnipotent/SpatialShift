using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    private float startPos, length;
    [SerializeField] private GameObject cam;
    [Range(0,1)] [SerializeField] private float parallaxEffectX;

    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log(gameObject.name + ": " + length);
    }

    private void Update()
    {
        float distance = cam.transform.position.x * parallaxEffectX;
        float movement = cam.transform.position.x * (1 - parallaxEffectX);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        /*
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
        */
    }
}
