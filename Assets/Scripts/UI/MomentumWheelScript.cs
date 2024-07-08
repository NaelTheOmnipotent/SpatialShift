using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MomentumWheelScript : MonoBehaviour
{
    //Variablse
    [HideInInspector] public float momentum;
    [SerializeField] private float momentumDecay;
    private float maxMomentum;
    private bool hasRun = false;
    private bool hasWaited;

    //References
    private Slider momentumWheel;
    [SerializeField] private Slider usageWheel;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private Image fill;

    private void Start()
    {
        //Gets the Slider Component
        momentumWheel = GetComponent<Slider>();
        
        //Sets a temporary maxMomentum that gets overriden shortly
        maxMomentum = 10;
    }

    // Update is called once per frame
    void Update()
    {
        bool isTeleporting = playerScript.hasTeleported;

        //If the player is Shifting
        if (isTeleporting)
        {
            //Only runs for the first time
            if (!hasRun)
            {
                maxMomentum = playerScript.trueVelocity;
                momentum = maxMomentum;
                hasRun = true;
            }

            //Calls the MomentumDelay Method if it hasn't been called yet
            if (!hasWaited)
            {
                StartCoroutine(MomentumDelay());
            }

            //reduces the momentum
            if (momentum > 0 && hasWaited)
            {
                momentum -= momentumDecay * Time.unscaledDeltaTime;
            }

            //the value as percentage
            usageWheel.value = momentum / maxMomentum + 0.05f;
        }
        else
        {
            //Fills the MomentumWheel back up
            if (momentum < maxMomentum) 
            { 
                momentum += 30 * Time.deltaTime;
            }

            //Resets values
            usageWheel.value = momentum / maxMomentum;
            hasRun = false;
            hasWaited = false;
        }

        //The value as percentage
        momentumWheel.value = momentum / maxMomentum;
        
        //If the player waits till the value is 0 the shift gets cut off
        if (momentum <= 0)
        {
            Time.timeScale = 1;
            playerScript.hasTeleported = false;
        }
    }

    IEnumerator MomentumDelay()
    {
        //How long until the player looses speed
        yield return new WaitForSecondsRealtime(.5f);
        hasWaited = true;
    }
}
