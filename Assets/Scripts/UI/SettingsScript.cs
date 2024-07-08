using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{
    //References
    private string fullscreenInPP = "Fullscreen";
    private int fullscreenInt = 1;

    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainAudioMixer;
    public TextMeshProUGUI masterVolumePercentage;
    public TextMeshProUGUI musicVolumePercentage;
    public TextMeshProUGUI sfxVolumePercentage;
    [SerializeField] private Toggle fullScreenToggle;

    private string masterVolumeName = "masterVolume";
    private float masterVolume;


    private void Start()
    {
        masterVol.value = PlayerPrefs.GetFloat("masterVolume", masterVolume);
    }


    public void FullScreenChange()
    {
        //will set the game to fullscreen oder windowed
        if (fullScreenToggle.isOn)
        {
            Debug.Log(true);
            Screen.fullScreen = true;
        }
        else
        {
            Debug.Log(false);
            Screen.fullScreen = false;
        }
    }
    
    public void ChangeMasterVolume()
    {
        //will change the mastervolume
        mainAudioMixer.SetFloat("MasterVolume", masterVol.value);
        var percentage = masterVol.value / -80;
        percentage = 1 - percentage;
        percentage *= 100;
        
        //will display the percentage
        if (percentage == 100)
        {
            masterVolumePercentage.text = percentage.ToString("000");
        }
        else if (percentage > 9)
        {
            masterVolumePercentage.text = percentage.ToString("00");
        }
        else
        {
            masterVolumePercentage.text = percentage.ToString("0");
        }
        
        PlayerPrefs.SetFloat("masterVolume", masterVolume);
    }
    public void ChangeMusicVolume()
    {
        //will change the musicvolume
        mainAudioMixer.SetFloat("MusicVolume", musicVol.value);
        
        var percentage = musicVol.value / -80;
        percentage = 1 - percentage;
        percentage *= 100;

        // will display the percentage
        if (percentage == 100)
        {
            musicVolumePercentage.text = percentage.ToString("000");
        }
        else if (percentage > 9)
        {
            musicVolumePercentage.text = percentage.ToString("00");
        }
        else
        {
            musicVolumePercentage.text = percentage.ToString("0");
        }
    }
    public void ChangeSFXVolume()
    {
        //will change the sfxvolume
        mainAudioMixer.SetFloat("SFXVolume", sfxVol.value);
        
        var percentage = sfxVol.value / -80;
        percentage = 1 - percentage;
        percentage *= 100;
        
        //display the percentage
        if (percentage == 100)
        {
            sfxVolumePercentage.text = percentage.ToString("000");
        }
        else if (percentage > 9)
        {
            sfxVolumePercentage.text = percentage.ToString("00");
        }
        else
        {
            sfxVolumePercentage.text = percentage.ToString("0");
        }
    }

  
    
}