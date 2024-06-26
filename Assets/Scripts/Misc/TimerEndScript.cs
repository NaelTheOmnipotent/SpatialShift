using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEndScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManagerScript.timerIsRunning = false;
        Debug.Log("Timer Ended");
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,transform.localScale);
    }
}
