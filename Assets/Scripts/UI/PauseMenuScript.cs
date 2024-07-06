using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private CanvasGroup pauseMenuCanvasGroup;
    [SerializeField] private GameManagerScript gameManager;

    //When the player presses the Continue Button
    public void ContinueButton()
    {
        pauseMenuCanvasGroup.HideCanvasGroup();
        countDownText.gameObject.SetActive(true);

        StartCoroutine(CountDown());
    }

    //Resets the Player to the start of the Level by calling the Reload Level Method in the GameManager
    public void RestartLevelButton()
    {
        gameManager.RestartLevel();
        pauseMenuCanvasGroup.HideCanvasGroup();
    }

    //Resets the Player to the last Checkpoint by calling the Reload Level Method in the GameManager
    public void RestartFromLastCheckpoint()
    {
        gameManager.RestartFromCheckpoint();
        pauseMenuCanvasGroup.HideCanvasGroup();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //The Countdown for when the player Closes the Pause Menu
    IEnumerator CountDown()
    {
        countDownText.text = "3";
        yield return new WaitForSecondsRealtime(1);
        
        countDownText.text = "2";
        yield return new WaitForSecondsRealtime(1);
        
        countDownText.text = "1";
        yield return new WaitForSecondsRealtime(1);
        
        Time.timeScale = 1;
        countDownText.gameObject.SetActive(false);
    }
}
