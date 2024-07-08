using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemySpawner : MonoBehaviour
{
    //references
    [SerializeField] private GameObject jumpingEnemy;
    [SerializeField] private float jumpHeight;

    private GameObject spawnedEnemy;

    private void OnBecameVisible()
    {
        //Spawns the Enemy if the spawner is on screen
        spawnedEnemy = Instantiate(jumpingEnemy, transform.position - new Vector3(0, transform.localScale.y / 2), Quaternion.identity);
        spawnedEnemy.GetComponent<JumpingEnemy>().jumpForce = jumpHeight * 2;   
    }

    private void OnBecameInvisible()
    {
        //Destroys the Enemy if the spawner is offscrean
        Destroy(spawnedEnemy);
    }

    private void OnDrawGizmos()
    {
        //Gizmos for level designing
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
