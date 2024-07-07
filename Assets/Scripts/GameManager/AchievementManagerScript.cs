using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManagerScript : MonoBehaviour
{
    [HideInInspector] public float timeAsScore;
    [SerializeField] private Vector2 startOfLevel;
    [SerializeField] private Vector2 endOfLevel;
    [SerializeField] private GameObject player;
    private bool reachedEndOfLevel;
    public static int enemiesKilled;
    
    public void OnLevelComplete()
    {
        if (timeAsScore < 30)
        {
            PlayerPrefs.SetInt("BlueHedgehogAchievement", 1);
            Debug.Log("AchievementCompleted");
        }

        if (timeAsScore < 60 && enemiesKilled >= 10)
        {
            PlayerPrefs.SetInt("CleansingAchievement", 1);
            Debug.Log("Achievement Reached!");
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
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(endOfLevel, 5);
        Gizmos.DrawWireSphere(startOfLevel, 5);
    }
}
