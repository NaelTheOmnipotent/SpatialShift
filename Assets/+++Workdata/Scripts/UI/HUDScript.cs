using TMPro;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    //References
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Rigidbody2D needle;
    
    //Variables for Timer
    [HideInInspector] public int minutes;
    [HideInInspector] public int seconds;
    [HideInInspector] public float milliSeconds;
    [HideInInspector] public float timeAsScore;
    
    private void Start()
    {
        //Gets the minutes stored in the static int
        minutes = GameManagerScript.minutes;
    }

    private void Update()
    {
        //Rotates the Needle around the tachometer
        var needleRotationTarget = 120 + -playerScript.rb.velocity.magnitude * 4;
        needle.rotation = Mathf.Lerp(needle.rotation, needleRotationTarget, Time.deltaTime * 5);
        
        //Calls the TimerText Method
        TimerText();
    }

    void TimerText()
    {
        //Rounds the Number to an int
        seconds = (int)GameManagerScript.currentTime;
        
        //When a minute is reached, add a minute and set seconds to 0
        if (seconds > 59)
        {
            seconds = 0;
            GameManagerScript.currentTime = 0;
            minutes += 1;
            GameManagerScript.minutes += 1;
        }
        
        //If 100 milliseconds have passed, reset it
        milliSeconds = (GameManagerScript.currentTime - (int)GameManagerScript.currentTime) * 100;
        if (milliSeconds > 99)
        {
            milliSeconds = 0;
        }
        
        //formats the text
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
        timeAsScore = minutes * 60 + seconds + milliSeconds / 100;
    }
}
