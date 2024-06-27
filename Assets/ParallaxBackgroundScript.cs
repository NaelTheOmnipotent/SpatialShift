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
        Vector3 camPosition = cam.transform.position;

        var targetPosition = new Vector3(transform.position.x, camPosition.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, dampeningEffect * Time.deltaTime);
    }
}
