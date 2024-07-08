using System.Collections;
using UnityEngine;

public class BlackScreenScript : MonoBehaviour
{
    //References
    private CanvasGroup blackScreen;
    private bool isFadingIn;

    private void Awake()
    {
        //Gets the Blackscreen and sets it to 1
        blackScreen = GetComponent<CanvasGroup>();
        blackScreen.alpha = 1;
    }

    private void Start()
    {
        //Starts the BlackScreenFadeOut Coroutine
        StartCoroutine(BlackScreenFadeOutDelay());
    }

    private void Update()
    {
        //Fades out the BlackScreen
        if (blackScreen.alpha != 0 && isFadingIn)
        {
            blackScreen.alpha -= Time.deltaTime;
        }
        else
        {
            //Turns of FadeIn
            isFadingIn = false;
        }
    }

    private IEnumerator BlackScreenFadeOutDelay()
    {
        //BlackScreen fadeInDelay
        yield return new WaitForSecondsRealtime(.3f);
        isFadingIn = true;
    }
}
