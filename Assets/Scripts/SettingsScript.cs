using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{
    private string fullscreenInPP = "Fullscreen";
    private int fullscreenInt = 1;

    public Slider masterVol, musicVol, sfxVol;
    public AudioMixer mainAudioMixer;
    public TextMeshProUGUI masterVolumePercentage;
    public TextMeshProUGUI musicVolumePercentage;
    public TextMeshProUGUI sfxVolumePercentage;

    private string masterVolumeName = "masterVolume";
    private float masterVolume;


    private void Start()
    {
        masterVol.value = PlayerPrefs.GetFloat("masterVolume", masterVolume);
    }


    public void SetFullscreen(bool isFullScreenOn)
    {
        if (isFullScreenOn)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }
    
    public void ChangeMasterVolume()
    {
        mainAudioMixer.SetFloat("MasterVolume", masterVol.value);
        var percentage = masterVol.value / -80;
        percentage = 1 - percentage;
        percentage *= 100;

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
        mainAudioMixer.SetFloat("MusicVolume", musicVol.value);
        
        var percentage = musicVol.value / -80;
        percentage = 1 - percentage;
        percentage *= 100;

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
        mainAudioMixer.SetFloat("SFXVolume", sfxVol.value);
        
        var percentage = sfxVol.value / -80;
        percentage = 1 - percentage;
        percentage *= 100;

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