using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundScript : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float dampeningEffect;
    private void Update()
    {
        float camPositionY = cam.transform.position.y;
        transform.position = new Vector3(transform.position.x, camPositionY, transform.position.z);

        /*
        var targetPosition = new Vector3(transform.position.x, camPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, dampeningEffect * Time.deltaTime);
        */
    }
}
