using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemySpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    private GameObject spawnedEnemy;

    [SerializeField] private float speed;
    [SerializeField] private bool hasGravity;

    private Vector2 pointA;
    private Vector2 pointB;

    private void Start()
    {
        pointA = new Vector2(transform.position.x + transform.localScale.x / 2, transform.position.y);
        pointB = new Vector2(transform.position.x - transform.localScale.x / 2, transform.position.y);
    }

    private void OnBecameVisible()
    {
        spawnedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        spawnedEnemy.GetComponent<HorizontalEnemyScript>().speed = speed;
        
        if (hasGravity)
        {
            spawnedEnemy.GetComponent<Rigidbody2D>().gravityScale = 2.5f;
        }
        else
        {
            spawnedEnemy.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(spawnedEnemy);
    }

    private void Update()
    {
        if (spawnedEnemy != null)
        {
            if (spawnedEnemy.transform.position.x > pointA.x)
            {
                if (Mathf.Sign(spawnedEnemy.GetComponent<HorizontalEnemyScript>().speed) > 0)
                {
                    spawnedEnemy.GetComponent<HorizontalEnemyScript>().speed *= -1;
                }
            }
            else if (spawnedEnemy.transform.position.x < pointB.x)
            {
                if (Mathf.Sign(spawnedEnemy.GetComponent<HorizontalEnemyScript>().speed) < 0)
                {
                    spawnedEnemy.GetComponent<HorizontalEnemyScript>().speed *= -1;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        Gizmos.DrawWireSphere(pointA,.1f);
        Gizmos.DrawWireSphere(pointB,.1f);
    }
}
