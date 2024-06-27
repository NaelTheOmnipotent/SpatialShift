using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SewerLightingScript : MonoBehaviour
{
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D entranceLight;
    [SerializeField] private Light2D exitLight;

    [SerializeField] private float targetSpotLightIntensity = 234.2f;
    [Range(0,1)][SerializeField] private float targetDarkness;
    [SerializeField] private float lightChangeSpeed;
    [SerializeField] private float spotLightDarknessChangeRate;
    [SerializeField] private float spotLightLightingChangeRate;
    private bool isDarkening; 
    private bool isLightingUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && globalLight.intensity == 1)
        {
            isDarkening = true;
            isLightingUp = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && globalLight.intensity < 1)
        {
            isLightingUp = true;
            isDarkening = false;
        }
    }

    private void Update()
    {
        if (isDarkening && globalLight.intensity > targetDarkness)
        {
            globalLight.intensity -= Time.deltaTime * lightChangeSpeed;
        }
        else if (globalLight.intensity < targetDarkness && isDarkening && entranceLight.intensity >= targetSpotLightIntensity)
        {
            isDarkening = false;
            Debug.Log(false);
        }

        if (isDarkening && entranceLight.intensity < targetSpotLightIntensity)
        {
            entranceLight.intensity = exitLight.intensity += Time.deltaTime * spotLightLightingChangeRate;
            Debug.Log("LightingUp");
        }
        else if (isLightingUp && entranceLight.intensity > 0)
        {
            entranceLight.intensity = exitLight.intensity -= Time.deltaTime * spotLightDarknessChangeRate;
            Debug.Log("Darkening");
        }

        if (isLightingUp && globalLight.intensity < 1)
        {
            globalLight.intensity += Time.deltaTime * lightChangeSpeed;
        }
        else if (isLightingUp && globalLight.intensity >= 1 && entranceLight.intensity <= 0)
        {
            isLightingUp = false;
            globalLight.intensity = 1;
        }

        if (exitLight.intensity < 0)
        {
            exitLight.intensity = 0;
        }
    }
}
