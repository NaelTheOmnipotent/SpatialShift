using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEndScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup scoreCanvasGroup;
    [SerializeField] private GameObject watchTower;
    private GameObject player;
    private Rigidbody2D playerRigidBody;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagerScript.timerIsRunning = false;
            scoreCanvasGroup.gameObject.GetComponent<ScoreScreenScript>().EndOfLevel();
            
            player = other.gameObject;
            playerRigidBody = other.attachedRigidbody;
            
            player.GetComponent<InputHandlerScript>().enabled = false;
            
            StartCoroutine(WaitForScoreboardToShowUp());
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,transform.localScale);
    }

    IEnumerator WaitForScoreboardToShowUp()
    {
        yield return new WaitUntil(() => playerRigidBody.velocity.x == 0);
        yield return new WaitForSeconds(1);
        
        if (player.transform.position.x > watchTower.transform.position.x)
        {
            player.transform.localScale = new Vector3(-Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y);
        }
        else if (player.transform.position.x > watchTower.transform.position.x)
        {
            player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y);
        }

        yield return new WaitForSeconds(2);
        
        scoreCanvasGroup.ShowCanvasGroup();
    }
}
