using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
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
  
  
   
  private void Start()
  {
    ShowCanvasGroup(mainMenuPanel);
    HideCanvasGroup(settingsPanel);
    HideCanvasGroup(extrasPanel);
  }

  public void OpenSettingsPanel()
  {
    StartCoroutine(transition());
  }

   
  public void CloseSettingsPanel()
  {
    StartCoroutine(TransitionToMain());
  }


  public void OpenExtrasPanel()
  {
    StartCoroutine(TransitionToExtras());
  }

  IEnumerator TransitionToExtras()
  {
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
    anim.SetTrigger("isGoingToSettings");
    HideCanvasGroup(mainMenuPanel);
    yield return new WaitForSeconds(0.75f);
    ShowCanvasGroup(settingsPanel);
    
  }
  IEnumerator TransitionToMain()
  {
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
    SceneManager.LoadScene("LevelOne");
  }

  public void QuitGame()
  {
    Application.Quit();
  }
  
  
  //Script for ExtrasPanel

  public void OpenControls()
  {
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
    ShowCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(controllerControlsActivated);
  }

  public void OpenControllerControls()
  {
    ShowCanvasGroup(controllerControlsActivated);
    HideCanvasGroup(keyboardControlsActivated);
  }

  public void OpenTips()
  {
    ShowCanvasGroup(tipsPanel);
    HideCanvasGroup(controlPanel);
    HideCanvasGroup(controllerControlsActivated);
    HideCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(achievementPanel);
    HideCanvasGroup(creditsPanel);
  }

  public void OpenAchievements()
  {
    ShowCanvasGroup(achievementPanel);
    HideCanvasGroup(controlPanel);
    HideCanvasGroup(controllerControlsActivated);
    HideCanvasGroup(keyboardControlsActivated);
    HideCanvasGroup(tipsPanel);
    HideCanvasGroup(creditsPanel);
  }

  public void OpenCredits()
  {
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
  
  void ShowCanvasGroup(CanvasGroup canvasGroup)
  {
    canvasGroup.alpha = 1f;
    canvasGroup.interactable = true;
    canvasGroup.blocksRaycasts = true;
  }

  
  void HideCanvasGroup(CanvasGroup canvasGroup)
  {
    canvasGroup.alpha = 0f;
    canvasGroup.interactable = false;
    canvasGroup.blocksRaycasts = false;

  }
  
}
