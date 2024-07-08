using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerEndScript : MonoBehaviour
{
    //References
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
            //Ends the timer and calls the EndOfLevel Method for the ScoreScreen
            GameManagerScript.timerIsRunning = false;
            scoreCanvasGroup.gameObject.GetComponent<ScoreScreenScript>().EndOfLevel();
            
            //Gets the player and rb
            player = other.gameObject;
            playerRigidBody = other.attachedRigidbody;
            
            //Deactivates Input
            player.GetComponent<InputHandlerScript>().enabled = false;
            
            //Calls Achievements and the Coroutine
            achievementManager.OnLevelComplete();
            StartCoroutine(WaitForScoreboardToShowUp());
        }
    }
    

    IEnumerator WaitForScoreboardToShowUp()
    {
        //Waits till the players speed is 0
        yield return new WaitUntil(() => playerRigidBody.velocity.x == 0);
        
        //Slowly Makes the hud dissappear
        while (hudCanvasGroup.alpha > 0)
        {
            hudCanvasGroup.alpha -= Time.deltaTime;
            
            //Makes sure it won't run infinitely and crash the game
            yield return null;
        }
        //waits a secodn
        yield return new WaitForSeconds(1);
        
        //Turns the player towards the watchtower
        if (player.transform.position.x > watchTower.transform.position.x)
        {
            player.transform.localScale = new Vector3(-Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y);
        }
        else if (player.transform.position.x > watchTower.transform.position.x)
        {
            player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y);
        }

        yield return new WaitForSeconds(.5f);
        
        //Shows the ResultScreen
        scoreCanvasGroup.ShowCanvasGroup();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,transform.localScale);
    }
}
