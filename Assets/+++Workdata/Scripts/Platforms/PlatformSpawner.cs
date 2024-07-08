using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    //Platform
    [SerializeField] private GameObject platform;
    
    //Variable which one it is
    [Space(15)]
    [SerializeField] private bool isFallThroughPlatform;
    [SerializeField] private bool isDestructiblePlatform;
    [SerializeField] private bool isDecayingPlatform;
    
    [Space(10)] 
    [SerializeField] private float decayTime;

    private GameObject spawnedPlatform;
    
    private void OnBecameVisible()
    {
        //Spawns a destructiblePlatform with the same transform as the spawner
        if (isDestructiblePlatform)
        {
            spawnedPlatform = Instantiate(platform, transform.position, Quaternion.identity);
            spawnedPlatform.transform.localScale = transform.lossyScale;
        }
        //Spawns a destructiblePlatform with the same transform as the spawner
        else if (isFallThroughPlatform)
        {
            spawnedPlatform = Instantiate(platform, transform.position, Quaternion.identity);
            spawnedPlatform.transform.localScale = transform.lossyScale;
        }
        //Spawns a destructiblePlatform with the same transform as the spawner
        else if (isDecayingPlatform)
        {
            spawnedPlatform = Instantiate(platform, transform.position, Quaternion.identity);
            spawnedPlatform.transform.localScale = transform.lossyScale;
            spawnedPlatform.GetComponent<DecayingPlatformScripts>().decayTime = decayTime;
        }
    }

    private void OnBecameInvisible()
    {
        //Destroy the spawned Platform
        Destroy(spawnedPlatform);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
