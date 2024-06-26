using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    
    [Space(15)]
    [SerializeField] private bool isFallThroughPlatform;
    [SerializeField] private bool isDestructiblePlatform;
    [SerializeField] private bool isDecayingPlatform;

    [Space(10)] 
    [SerializeField] private float decayTime;

    private GameObject spawnedPlatform;
    
    private void OnBecameVisible()
    {
        if (isDestructiblePlatform)
        {
            spawnedPlatform = Instantiate(platform, transform.position, Quaternion.identity);
            spawnedPlatform.transform.localScale = transform.lossyScale;
        }
        else if (isFallThroughPlatform)
        {
            spawnedPlatform = Instantiate(platform, transform.position, Quaternion.identity);
            spawnedPlatform.transform.localScale = transform.lossyScale;
        }
        else if (isDecayingPlatform)
        {
            spawnedPlatform = Instantiate(platform, transform.position, Quaternion.identity);
            spawnedPlatform.transform.localScale = transform.lossyScale;
            spawnedPlatform.GetComponent<DecayingPlatformScripts>().decayTime = decayTime;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(spawnedPlatform);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
