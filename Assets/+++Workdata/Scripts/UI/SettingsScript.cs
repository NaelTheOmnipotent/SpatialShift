using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{
    [Header("___Game Runs for the first time___")]
    private String gameInPP = "is the Game running for the first time"; //gets written in the Player Prefabs
    private int gameInt = 0;

    
    
    [Header("___Fullscreen Toggle___")] 
    private String fullScreenInPP = "Fullscreen";
    private int fullScreenInt = 1;

    [Header("AudioSlider")] 
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public TextMeshProUGUI masterVolumePercentage;
    public Slider musicVolumeSlider;
    public TextMeshProUGUI musicVolumePercentage;
    public Slider sfxVolumeSlider;
    public TextMeshProUGUI sfxVolumePercentage;
    
    //PlayerPrefabs
    private string masterVolumeInPP = "masterVolume";
    private float masterVolume;
    private string musicVolumeInPP = "musicVolume";
    private float musicVolume;
    private string sfxVolumeInPP = "sfxVolume";
    private float sfxVolume;

    private float defaultMasterVolume = 0.6f;
    private float defaultMusicVolume = 1f;
    private float defaultSFXVolume = 0.5f;

    private float tempMasterVolume;
    private float tempMusicVolume;
    private float tempSFXVolume;
    
    public void Start()
    {
        //FruitCount();
        CheckIfTheGameRunsForTheFirstTime();
    }

    // Checks if the game is running for the first time
    public void CheckIfTheGameRunsForTheFirstTime()
    {
        if (gameInt == 0)
        {
            gameInt = 1;
            
            //Saves the Gameint Variable in the player prefs
            PlayerPrefs.SetInt(gameInPP,gameInt);
            
            masterVolumeSlider.value = masterVolume = defaultMasterVolume;
            musicVolumeSlider.value = musicVolume = defaultMusicVolume;
            sfxVolumeSlider.value = sfxVolume = defaultSFXVolume;
            
            PlayerPrefs.SetFloat(masterVolumeInPP, masterVolume);
            PlayerPrefs.SetFloat(musicVolumeInPP, musicVolume);
            PlayerPrefs.SetFloat(sfxVolumeInPP,sfxVolume);
            
        }
        else  //Loads the saved settings
        {
            gameInt = PlayerPrefs.GetInt(gameInPP, gameInt);
            fullScreenInt = PlayerPrefs.GetInt(fullScreenInPP, fullScreenInt);
            
            PlayerPrefs.GetFloat(masterVolumeInPP, masterVolume);
            PlayerPrefs.GetFloat(musicVolumeInPP, musicVolume);
            PlayerPrefs.GetFloat(sfxVolumeInPP,sfxVolume);

            if (fullScreenInt == 1)
            {
                SetFullScreenMode(true);
            }
            else
            {
                SetFullScreenMode(false);
            }
        }
        
    }

    //converts The bool isFullScreenOn to int for player preFab
    public void SetFullScreenMode(bool isFullScreenOn)
    {
        // set fullscreen mode
        Screen.fullScreen = isFullScreenOn;

        if (isFullScreenOn == true)
        {
            fullScreenInt = 1;
        }
        else
        {
            fullScreenInt = 0;
        }
        
        PlayerPrefs.GetInt(fullScreenInPP, fullScreenInt);
    }
    
    public void SaveSettings()
    {
        tempMasterVolume = masterVolumeSlider.value;
        tempMusicVolume = musicVolumeSlider.value;
        tempSFXVolume = sfxVolumeSlider.value;
    }

    public void ApplyTempSettings()
    {
        masterVolumeSlider.value = tempMasterVolume;
        musicVolumeSlider.value = tempMusicVolume;
        sfxVolumeSlider.value = tempSFXVolume;
    }

    public void ApplyButton()
    {
        tempMasterVolume = masterVolumeSlider.value;
        tempMusicVolume = musicVolumeSlider.value;
        tempSFXVolume = sfxVolumeSlider.value;
        
        PlayerPrefs.SetFloat(masterVolumeInPP, masterVolume);
        PlayerPrefs.SetFloat(musicVolumeInPP, musicVolume);
        PlayerPrefs.SetFloat(sfxVolumeInPP,sfxVolume);
    }

    public void ChangeMasterVolume()
    {
        masterVolume = masterVolumeSlider.value;
        
        
        audioMixer.SetFloat("MasterVolume", masterVolume);

        masterVolumePercentage.text = ((masterVolume * 100).ToString("0") + "%");
        
        PlayerPrefs.SetFloat(masterVolumeInPP, masterVolume);
    }
    
    public void ChangeMusicVolume()
    {
        musicVolume = musicVolumeSlider.value;

        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);

        musicVolumePercentage.text = ((musicVolume * 100).ToString("0") + "%");
        
        PlayerPrefs.SetFloat(musicVolumeInPP, musicVolume);
    }
    
    public void ChangeSFXVolume()
    {
        sfxVolume = sfxVolumeSlider.value;

        audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);

        sfxVolumePercentage.text = ((sfxVolume * 100).ToString("0") + "%");
        
        PlayerPrefs.SetFloat(sfxVolumeInPP, sfxVolume);
    }

    public void RevertToDefault()
    {
        masterVolumeSlider.value = defaultMasterVolume;
        musicVolumeSlider.value = defaultMusicVolume;
        sfxVolumeSlider.value = defaultSFXVolume;
    }
}