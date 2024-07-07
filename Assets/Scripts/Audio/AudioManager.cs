using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   
   [Header("------Audio Source------")]
   [SerializeField] private AudioSource musicSource;
   [SerializeField] private AudioSource sfxSource;

   [Header("------Audio Clip------")]
   public AudioClip checkpoint;
   public AudioClip breakingwood;
   public AudioClip takingdamage;
   public AudioClip jump;
   public AudioClip jumpenemy;
   public AudioClip runninggrassSlow;
   public AudioClip runninggrassMid;
   public AudioClip runnningstonSlow;
   public AudioClip runningStoneMid;
   public AudioClip shift;
   public AudioClip dashAterShift;
   public AudioClip stomp;
   public AudioClip backgorund;

   private void Start()
   {
      musicSource.clip = backgorund;
      musicSource.Play();
   }

   public void PlaySFX(AudioClip clip)
   {
      sfxSource.PlayOneShot(clip);
   }
}
