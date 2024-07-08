using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManagerScript : MonoBehaviour
{
    //Variables/References
    [HideInInspector] public float timeAsScore;
    [SerializeField] private Vector2 startOfLevel;
    [SerializeField] private Vector2 endOfLevel;
    [SerializeField] private GameObject player;
    
    private bool reachedEndOfLevel;
    public static int enemiesKilled;

    //Reference to TMP
    [SerializeField] private Animator achievementAnimator;
    [SerializeField] private TextMeshProUGUI achievementText;
    
    public void OnLevelComplete()
    {
        //If the player finishes under 30 seconds
        if (timeAsScore < 30)
        {
            //Saves the Achievement
            PlayerPrefs.SetInt("BlueHedgehogAchievement", 1);
            
            //Makes a notification appear in the bottom left
            achievementText.text = "Blue Hedgehog";
            StartCoroutine(AchievementNotification());
        }
        //If the player finishes under 60 seconds and has killed 10 enemies
        if (timeAsScore < 60 && enemiesKilled >= 10)
        {
            //Saves the Achievement
            PlayerPrefs.SetInt("CleansingAchievement", 1);
            
            //Makes a notification appear in the bottom left
            achievementText.text = "Cleansing";
            StartCoroutine(AchievementNotification());
        }
    }

    private void Update()
    {
        //If there is a reference to the player and the achievement hasn't been gotten
        if (player != null && PlayerPrefs.GetInt("ColdFeetAchievement") != 1)
        {
            //Checks if the player has been further the what is considered the end of the level
            if (player.transform.position.x > endOfLevel.x)
            {
                reachedEndOfLevel = true;
            }

            //If the player get back to the start of the level the achievement is granted
            if (player.transform.position.x < startOfLevel.x && reachedEndOfLevel)
            {
                //Saves the Achievement
                PlayerPrefs.SetInt("ColdFeetAchievement", 1);
                Debug.Log("Achievement Reached!");

                //Makes a notification appear in the bottom left
                achievementText.text = "Cold Feet";
                StartCoroutine(AchievementNotification());
            }
        }
    }

    private IEnumerator AchievementNotification()
    {
        //Achievement NotificationAnimations
        achievementAnimator.SetTrigger("AnimationStart");
        yield return new WaitForSecondsRealtime(5);
        achievementAnimator.SetTrigger("AnimationFinished");
    }
    
    private void OnDrawGizmos()
    {
        //Gizmos
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endOfLevel, 5);
        Gizmos.DrawWireSphere(startOfLevel, 5);
    }
}
