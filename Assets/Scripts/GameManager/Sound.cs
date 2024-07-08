using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    //Variables to change the audioClips settings
    [HideInInspector] public AudioSource source;
    
    public string name;
    public AudioClip clip;
    
    [Space(10)]
    
    [Range(0, 1)] public float volume = 1;
    [Range(-3, 3)] public float pitch = 1;
    
    [Space(10)]
    
    public bool loop;

    //public AudioMixerGroup audioMixer;
}
