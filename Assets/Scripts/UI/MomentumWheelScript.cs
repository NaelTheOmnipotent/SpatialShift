using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MomentumWheelScript : MonoBehaviour
{
    [HideInInspector] public float momentum;
    private float maxMomentum;
    [SerializeField] private float momentumDecay;
    private bool hasRun = false;
    private bool hasWaited;

    private Slider momentumWheel;
    [SerializeField] private Slider usageWheel;
    [SerializeField] private PlayerScript playerScript;
    [SerializeField] private Image fill;

    private float red;
    private float green;
    private float blue;


    private void Start()
    {
        momentumWheel = GetComponent<Slider>();
        
        //momentumCanvas.enabled = false;
        maxMomentum = 10;
    }

    // Update is called once per frame
    void Update()
    {

        bool isTeleporting = playerScript.hasTeleported;

        if (isTeleporting)
        {
//            momentumCanvas.enabled = true;

            if (!hasRun)
            {
                maxMomentum = playerScript.trueVelocity;
                momentum = maxMomentum;
                hasRun = true;
            }

            if (!hasWaited)
            {
                StartCoroutine(MomentumDelay());
            }

            if (momentum > 0 && hasWaited)
            {
                momentum -= momentumDecay * Time.unscaledDeltaTime;
            }

            usageWheel.value = momentum / maxMomentum + 0.05f;
        }
        else
        {
            if (momentum < maxMomentum) 
            { 
                momentum += 30 * Time.deltaTime;
            }

            usageWheel.value = momentum / maxMomentum;
            hasRun = false;
            hasWaited = false;
            red = blue = green = 0;
        }

        momentumWheel.value = momentum / maxMomentum;
        

        if (momentum <= 0)
        {
            Time.timeScale = 1;
            playerScript.hasTeleported = false;
            red = blue = green = 0;
        }
    }

    IEnumerator MomentumDelay()
    {
        yield return new WaitForSecondsRealtime(.5f);
        hasWaited = true;
    }
}
