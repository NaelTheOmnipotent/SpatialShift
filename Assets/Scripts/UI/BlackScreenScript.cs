using System;
using System.Collections;
using UnityEngine;

public class BlackScreenScript : MonoBehaviour
{
    private CanvasGroup blackScreen;
    private bool isFadingIn;
    [HideInInspector] public bool isFadingOut;

    private void Awake()
    {
        blackScreen = GetComponent<CanvasGroup>();
        blackScreen.alpha = 1;
        
        //isFadingIn = true;
    }

    private void Start()
    {
        StartCoroutine(BlackScreenFadeOutDelay());
    }

    private void Update()
    {
        if (blackScreen.alpha != 0 && isFadingIn)
        {
            blackScreen.alpha -= Time.deltaTime;
        }
        else
        {
            isFadingIn = false;
        }

        if (blackScreen.alpha <= 0 && isFadingOut)
        {
            blackScreen.alpha += Time.deltaTime;
        }
    }

    private IEnumerator BlackScreenFadeOutDelay()
    {
        yield return new WaitForSecondsRealtime(.2f);
        isFadingIn = true;
    }
}
