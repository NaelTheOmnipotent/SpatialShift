using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuScript : MonoBehaviour
{
    //References
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private CanvasGroup pauseMenuCanvasGroup;
    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private CanvasGroup extrasPanel;

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
        //Cals the BackToMainMenu Method
        gameManager.BackToMainMenu();
    }
    
    public void OpenExtras()
    {
        pauseMenuCanvasGroup.HideCanvasGroup();
        extrasPanel.ShowCanvasGroup();
    }
    public void BackToPause()
    {
        pauseMenuCanvasGroup.ShowCanvasGroup();
        extrasPanel.HideCanvasGroup();
    }

    //The Countdown for when the player Closes the Pause Menu and adds Controller Rumble
    IEnumerator CountDown()
    {
        countDownText.text = "3";
        StartCoroutine(ControllerRumble(.15f, .25f));
        yield return new WaitForSecondsRealtime(1);
        
        countDownText.text = "2";
        StartCoroutine(ControllerRumble(.2f, .25f));
        yield return new WaitForSecondsRealtime(1);
        
        countDownText.text = "1";
        StartCoroutine(ControllerRumble(.25f, .25f));
        yield return new WaitForSecondsRealtime(1);
        
        Time.timeScale = 1;
        countDownText.gameObject.SetActive(false);
        StartCoroutine(ControllerRumble(.35f, .5f));
    }

    IEnumerator ControllerRumble(float rumbleStrength, float length)
    {
        //Gets the MotorSpeed passed down and if the Controller isn't null sets the MotorSpeed to it
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(rumbleStrength, rumbleStrength);

            yield return new WaitForSecondsRealtime(length);
            
            Gamepad.current.SetMotorSpeeds(0,0);
        }
    }
}
