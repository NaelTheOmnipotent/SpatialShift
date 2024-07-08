using System;
using UnityEngine;

public class UIControllerScript : MonoBehaviour
{
    //References
    [SerializeField] private CanvasGroup hudCanvasGroup;
    [SerializeField] private CanvasGroup pauseMenuCanvasGroup;
    [SerializeField] private CanvasGroup leaderboardCanvasGroup;
    [SerializeField] private InputHandlerScript inputHandler;
    [SerializeField] private CanvasGroup extrasPanel;
    
    private void Start()
    {
        //Hide And Show CanvasGroups
        hudCanvasGroup.ShowCanvasGroup();
        pauseMenuCanvasGroup.HideCanvasGroup();
        leaderboardCanvasGroup.HideCanvasGroup();
        extrasPanel.HideCanvasGroup();
    }

    private void Update()
    {
        // If the Player presses the pause Button
        if (inputHandler.OnPauseMenu())
        {
            if (!pauseMenuCanvasGroup.interactable && Time.timeScale == 1)
            {
                //Activates the pause Menu
                pauseMenuCanvasGroup.ShowCanvasGroup();
                Time.timeScale = 0;
            }
            else
            {
                //If the button is pressed again, the PauseMenu is deactivated
                pauseMenuCanvasGroup.gameObject.GetComponent<PauseMenuScript>().ContinueButton();
            }
        }
        
    }

   
}

//a public static class can be accessed from everywhere and can only contain static Methods
public static class ExtentionMethod
{
    
    //When called it accesses the CanvasGroup that called it and sets its variables 
    public static void ShowCanvasGroup(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    //When called it accesses the CanvasGroup that called it and sets its variables 
    public static void HideCanvasGroup(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}