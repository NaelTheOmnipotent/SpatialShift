using TMPro;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private MomentumWheelScript momentumWheelScript;
    private PlayerInteractionScript playerInteraction;
    
    [SerializeField] private TextMeshProUGUI velocityText;
    [SerializeField] private TextMeshProUGUI coinTextAmount;
    [SerializeField] private TextMeshProUGUI timerText;
    
    int minutes;
    int seconds;
    float milliSeconds;

    private void Start()
    {
        playerInteraction = playerScript.gameObject.GetComponent<PlayerInteractionScript>();
        minutes = GameManagerScript.minutes;
    }

    private void Update()
    {
        if (!playerScript.hasTeleported)
        {
            velocityText.text = playerScript.rb.velocity.magnitude.ToString("00");
        }
        else
        {
            velocityText.text = momentumWheelScript.momentum.ToString("00");
        }
        coinTextAmount.text = playerInteraction.coinCount.ToString("");
        
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
    }
}
