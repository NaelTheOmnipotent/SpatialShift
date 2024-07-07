using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEndScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup scoreCanvasGroup;
    [SerializeField] private CanvasGroup hudCanvasGroup;
    [SerializeField] private GameObject watchTower;
    [SerializeField] private AchievementManagerScript achievementManager;
    
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
    

    IEnumerator WaitForScoreboardToShowUp()
    {
        yield return new WaitUntil(() => playerRigidBody.velocity.x == 0);
        
        while (hudCanvasGroup.alpha > 0)
        {
            hudCanvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }
        achievementManager.OnLevelComplete();
        yield return new WaitForSeconds(1);
        
        if (player.transform.position.x > watchTower.transform.position.x)
        {
            player.transform.localScale = new Vector3(-Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y);
        }
        else if (player.transform.position.x > watchTower.transform.position.x)
        {
            player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y);
        }

        yield return new WaitForSeconds(.5f);
        
        scoreCanvasGroup.ShowCanvasGroup();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,transform.localScale);
    }
}
