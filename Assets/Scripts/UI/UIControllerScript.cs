using System;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup hudCanvasGroup;
    [SerializeField] private CanvasGroup pauseMenuCanvasGroup;
    [SerializeField] private CanvasGroup leaderboardCanvasGroup;
    
    [SerializeField] private InputHandlerScript inputHandler;
    private void Start()
    {
        hudCanvasGroup.ShowCanvasGroup();
        pauseMenuCanvasGroup.HideCanvasGroup();
        leaderboardCanvasGroup.HideCanvasGroup();
    }

    private void Update()
    {
        if (inputHandler.OnPauseMenu())
        {
            if (!pauseMenuCanvasGroup.interactable && Time.timeScale == 1)
            {
                pauseMenuCanvasGroup.ShowCanvasGroup();
                Time.timeScale = 0;
            }
            else
            {
                pauseMenuCanvasGroup.gameObject.GetComponent<PauseMenuScript>().ContinueButton();
            }
        }
        
    }
}


public static class ExtentionMethod
{
    public static void ShowCanvasGroup(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    public static void HideCanvasGroup(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}