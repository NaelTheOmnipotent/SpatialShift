using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreenScript : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartLevelButton;
    
    [SerializeField] private TextMeshProUGUI playerTime;
    [SerializeField] private TextMeshProUGUI bestPlayerTime;

    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private HUDScript hud;

    private string personalBestInPP = "personalBest";
    private float personalBest;

    private string personalBestTimeMinutesInPP = "bestMinutes";
    private string personalBestTimeSecondsInPP = "bestSeconds";
    private string personalBestTimeMilliSecondsInPP = "bestMilliSeconds";

    private int personalBestMinutes;
    private int personalBestSeconds;
    private float personalBestMilliSeconds;
    
    public void RestartLevel()
    {
        gameManager.RestartLevel();
    }

    public void BackToMainMenu()
    {
        gameManager.BackToMainMenu();
    }

    public void EndOfLevel()
    {
        playerTime.text = string.Format("{0:00}:{1:00}:{2:00}", hud.minutes, hud.seconds, hud.milliSeconds);
        gameManager.gameObject.GetComponent<AchievementManagerScript>().timeAsScore = hud.timeAsScore;
        
        Debug.Log(hud.timeAsScore);
        //If this run is faster then the personal best
        if (hud.timeAsScore <= PlayerPrefs.GetFloat(personalBestInPP, personalBest) || PlayerPrefs.GetFloat(personalBestInPP, personalBest) == 0)
        {
            personalBest = hud.timeAsScore;
            PlayerPrefs.SetFloat(personalBestInPP, personalBest);
            bestPlayerTime.text = string.Format("{0:00}:{1:00}:{2:00}", hud.minutes, hud.seconds, hud.milliSeconds);

            personalBestMinutes = hud.minutes;
            personalBestSeconds = hud.seconds;
            personalBestMilliSeconds = hud.milliSeconds;
            
            
            PlayerPrefs.SetInt(personalBestTimeMinutesInPP, personalBestMinutes);
            PlayerPrefs.SetInt(personalBestTimeSecondsInPP, personalBestSeconds);
            PlayerPrefs.SetFloat(personalBestTimeMilliSecondsInPP, personalBestMilliSeconds);
        }
        //Else execute this
        else
        {
            bestPlayerTime.text = string.Format("{0:00}:{1:00}:{2:00}", PlayerPrefs.GetInt(personalBestTimeMinutesInPP, personalBestMinutes), PlayerPrefs.GetInt(personalBestTimeSecondsInPP, personalBestSeconds), PlayerPrefs.GetFloat(personalBestTimeMilliSecondsInPP,personalBestMilliSeconds));
        }
    }
}
