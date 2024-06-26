using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    private Material mat;
    private float distance;

    [Range(0, 05f)] private float speed;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        distance += Time.deltaTime * speed;
        mat.SetTextureOffset("MainTex", Vector2.right * distance);
    }
}
