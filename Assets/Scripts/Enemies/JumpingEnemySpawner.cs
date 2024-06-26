using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject jumpingEnemy;
    [SerializeField] private float jumpHeight;

    private GameObject spawnedEnemy;

    private void OnBecameVisible()
    {
        spawnedEnemy = Instantiate(jumpingEnemy, transform.position - new Vector3(0, transform.localScale.y / 2), Quaternion.identity);
        spawnedEnemy.GetComponent<JumpingEnemy>().jumpForce = jumpHeight * 2;   
    }

    private void OnBecameInvisible()
    {
        Destroy(spawnedEnemy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
