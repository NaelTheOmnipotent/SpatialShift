using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreenScript : MonoBehaviour
{
    //References
    [SerializeField] private TextMeshProUGUI playerTime;
    [SerializeField] private TextMeshProUGUI bestPlayerTime;

    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private HUDScript hud;

    //PlayerPrefs for best times
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
        //Restarts Level
        gameManager.RestartLevel();
    }

    public void BackToMainMenu()
    {
        //Goes back to the main menu
        gameManager.BackToMainMenu();
    }

    public void EndOfLevel()
    {
        //Displays the time in the correct format and passes down the time to the AchievementManager
        playerTime.text = string.Format("{0:00}:{1:00}:{2:00}", hud.minutes, hud.seconds, hud.milliSeconds);
        gameManager.gameObject.GetComponent<AchievementManagerScript>().timeAsScore = hud.timeAsScore;
        
        //If this run is faster then the personal best
        if (hud.timeAsScore <= PlayerPrefs.GetFloat(personalBestInPP, personalBest) || PlayerPrefs.GetFloat(personalBestInPP, personalBest) == 0)
        {
            //Updates the new Best time and saves it
            personalBest = hud.timeAsScore;
            PlayerPrefs.SetFloat(personalBestInPP, personalBest);
            bestPlayerTime.text = string.Format("{0:00}:{1:00}:{2:00}", hud.minutes, hud.seconds, hud.milliSeconds);

            //Updates the new variables to save them
            personalBestMinutes = hud.minutes;
            personalBestSeconds = hud.seconds;
            personalBestMilliSeconds = hud.milliSeconds;
            
            //saves them
            PlayerPrefs.SetInt(personalBestTimeMinutesInPP, personalBestMinutes);
            PlayerPrefs.SetInt(personalBestTimeSecondsInPP, personalBestSeconds);
            PlayerPrefs.SetFloat(personalBestTimeMilliSecondsInPP, personalBestMilliSeconds);
        }
        //Else execute this
        else
        {
            //Displays the best time
            bestPlayerTime.text = string.Format("{0:00}:{1:00}:{2:00}", PlayerPrefs.GetInt(personalBestTimeMinutesInPP, personalBestMinutes), PlayerPrefs.GetInt(personalBestTimeSecondsInPP, personalBestSeconds), PlayerPrefs.GetFloat(personalBestTimeMilliSecondsInPP,personalBestMilliSeconds));
        }
    }
}
