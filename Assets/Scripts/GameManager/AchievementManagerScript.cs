using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManagerScript : MonoBehaviour
{
    [HideInInspector] public float timeAsScore;
    [SerializeField] private Vector2 startOfLevel;
    [SerializeField] private Vector2 endOfLevel;
    [SerializeField] private GameObject player;
    
    private bool reachedEndOfLevel;
    public static int enemiesKilled;

    [SerializeField] private Animator achievementAnimator;
    [SerializeField] private TextMeshProUGUI achievementText;
    
    public void OnLevelComplete()
    {
        if (timeAsScore < 60)
        {
            PlayerPrefs.SetInt("BlueHedgehogAchievement", 1);
            Debug.Log("AchievementCompleted");

            achievementText.text = "Blue Hedgehog";
            StartCoroutine(AchievementNotification());
        }

        if (timeAsScore < 60 && enemiesKilled >= 10)
        {
            PlayerPrefs.SetInt("CleansingAchievement", 1);
            Debug.Log("Achievement Reached!");

            achievementText.text = "Cleansing";
            StartCoroutine(AchievementNotification());
        }
    }

    private void Update()
    {
        if (player != null && PlayerPrefs.GetInt("ColdFeetAchievement") != 1)
        {
            if (player.transform.position.x > endOfLevel.x)
            {
                reachedEndOfLevel = true;
            }

            if (player.transform.position.x < startOfLevel.x && reachedEndOfLevel)
            {
                PlayerPrefs.SetInt("ColdFeetAchievement", 1);
                Debug.Log("Achievement Reached!");

                achievementText.text = "Cold Feet";
                StartCoroutine(AchievementNotification());
            }
        }
    }

    private IEnumerator AchievementNotification()
    {
        achievementAnimator.SetTrigger("AnimationStart");
        Debug.Log(true);
        yield return new WaitForSecondsRealtime(5);
        achievementAnimator.SetTrigger("AnimationFinished");
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endOfLevel, 5);
        Gizmos.DrawWireSphere(startOfLevel, 5);
    }
}
