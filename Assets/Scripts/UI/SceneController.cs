using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
  //References  
  public Animator anim;
  public Animator credditsAnim;
  
  [Header("___MainMenuPanel___")] 
  [SerializeField] private CanvasGroup mainMenuPanel;
  [SerializeField] private CanvasGroup settingsPanel;
  [SerializeField] private CanvasGroup extrasPanel;
  
  

  [Header("___ExtrasSelectionPanel___")] 
  [SerializeField] private CanvasGroup controlPanel;
  [SerializeField] private CanvasGroup tipsPanel;
  [SerializeField] private CanvasGroup achievementPanel;
  [SerializeField] private CanvasGroup creditsPanel;

  [SerializeField] private CanvasGroup keyboardControlsActivated;
  [SerializeField] private CanvasGroup controllerControlsActivated;
  [SerializeField] private CanvasGroup showKeyBoardButton;
  [SerializeField] private CanvasGroup showControllerButton;

  [Header("__AchievementCheckmarks__")] 
  [SerializeField] private Image checkmarkBlueHedgehog;
  [SerializeField] private Image checkmarkCleansing;
  [SerializeField] private Image checkmarkColdFeet;
  
  private void Start()
  {
      //Extras Panel and Settings Panel will be invisible after starting the game
      //Main Menu Panel will be visible
    
      ShowCanvasGroup(mainMenuPanel);
      HideCanvasGroup(settingsPanel); 
      HideCanvasGroup(extrasPanel);
      
      
      if (PlayerPrefs.GetInt("BlueHedgehogAchievement") == 1)
      { 
        checkmarkBlueHedgehog.enabled = true;
      }
      
      if (PlayerPrefs.GetInt("CleansingAchievement") == 1) 
      { 
        checkmarkCleansing.enabled = true; 
      }

      if (PlayerPrefs.GetInt("ColdFeetAchievement") == 1)
      { 
        checkmarkColdFeet.enabled = true;
      }
  }

  //function for the Options Button
  public void OpenSettingsPanel()
  {
      StartCoroutine(transition());
  }

   //function for Back Buttons
  public void CloseSettingsPanel()
  {
      StartCoroutine(TransitionToMain());
  }


  //function for Extras Button
  public void OpenExtrasPanel()
  {
      StartCoroutine(TransitionToExtras());
  }

  IEnumerator TransitionToExtras()
  {
    //Will trigger the background animation "isGoingToSettings"
    //All Panels except the ExtrasPanel will be invisible
    anim.SetTrigger("isGoingToSettings");
    HideCanvasGroup(mainMenuPanel);
    HideCanvasGroup(settingsPanel);
    HideCanvasGroup(achievementPanel);
    HideCanvasGroup(tipsPanel);
    HideCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(controllerControlsActivated);
    HideCanvasGroup(showControllerButton);
    HideCanvasGroup(showKeyBoardButton);
    HideCanvasGroup(creditsPanel);
    yield return new WaitForSeconds(0.75f);
    ShowCanvasGroup(extrasPanel);
   
  }
  IEnumerator transition()
  {
    //will trigger the background transition animation
    //will hide MainMenuPanel and show SettingsPanel
    anim.SetTrigger("isGoingToSettings");
    HideCanvasGroup(mainMenuPanel);
    yield return new WaitForSeconds(0.75f);
    ShowCanvasGroup(settingsPanel);
    
  }
  IEnumerator TransitionToMain()
  {
    //will trigger the background animation from settings or extras to main menu
    //will hide every panel except the MainMenuPanel
    anim.SetTrigger("isGoingToMain");
    HideCanvasGroup(settingsPanel);
    HideCanvasGroup(extrasPanel);
    HideCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(controllerControlsActivated);
    HideCanvasGroup(creditsPanel);
    HideCanvasGroup(tipsPanel);
    HideCanvasGroup(achievementPanel);
    yield return new WaitForSeconds(0.75f);
    ShowCanvasGroup(mainMenuPanel);
    
  }
  
  public void PlayGame()
  {
    //Level scene will be loaded
    GameManagerScript.checkPointPosition = new Vector3(-9, -1, 0);
    SceneManager.LoadScene("LevelOne");
  }

  public void QuitGame()
  {
    //application will be closed
    Application.Quit();
  }
  
  
  //Script for ExtrasPanel

  public void OpenControls()
  {
    //ControlPanel will be shown
    ShowCanvasGroup(controlPanel);
    ShowCanvasGroup(showControllerButton);
    ShowCanvasGroup(showKeyBoardButton);
    ShowCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(creditsPanel);
    HideCanvasGroup(achievementPanel);
    HideCanvasGroup(tipsPanel);
    HideCanvasGroup(controllerControlsActivated);
    
  }

  public void OpenKeyBoardControls()
  {
    //will show the keyboard controls and hide controller controls
    ShowCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(controllerControlsActivated);
  }

  public void OpenControllerControls()
  {
    //will show the controller controls and hide keyboard controls
    ShowCanvasGroup(controllerControlsActivated);
    HideCanvasGroup(keyboardControlsActivated);
  }

  public void OpenTips()
  {
    //only tipsPanel will be visible
    ShowCanvasGroup(tipsPanel);
    HideCanvasGroup(controlPanel);
    HideCanvasGroup(controllerControlsActivated);
    HideCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(achievementPanel);
    HideCanvasGroup(creditsPanel);
  }

  public void OpenAchievements()
  {
    //only AchievementsPanel will be visible
    ShowCanvasGroup(achievementPanel);
    HideCanvasGroup(controlPanel);
    HideCanvasGroup(controllerControlsActivated);
    HideCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(tipsPanel);
    HideCanvasGroup(creditsPanel);
  }

  public void OpenCredits()
  {
    //CreditsPanel will be visible
    //Animation for the Credits will be played
    ShowCanvasGroup(creditsPanel);
    HideCanvasGroup(controlPanel);
    HideCanvasGroup(showControllerButton);
    HideCanvasGroup(showKeyBoardButton);
    HideCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(achievementPanel);
    HideCanvasGroup(tipsPanel);
    HideCanvasGroup(controllerControlsActivated);
    credditsAnim.SetTrigger("CreditsShowRoles");
  }
  
  
  //switch alpha of the canvas group to 1 = visible
  //will make the canvas group interactbale 
  void ShowCanvasGroup(CanvasGroup canvasGroup)
  {
    canvasGroup.alpha = 1f;
    canvasGroup.interactable = true;
    canvasGroup.blocksRaycasts = true;
  }

  
  //switch alpha of the canvas group to 0 = invisible 
  //user cant interact with the canvas group
  void HideCanvasGroup(CanvasGroup canvasGroup)
  {
    canvasGroup.alpha = 0f;
    canvasGroup.interactable = false;
    canvasGroup.blocksRaycasts = false;

  }
  
}
