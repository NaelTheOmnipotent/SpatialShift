using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Accesses the SoundScript as an Array
    [SerializeField] private Sound[] sounds;

    //Turns the AudioManager into a singleton (there can only be one per scene)
    public static AudioManager instance;
    private void Awake()
    {
        //DontDestroyOnLoad
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
        //Creates AudioSources with corresponding settings
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        //Plays the sound
       Sound s = Array.Find(sounds, sound => sound.name == name);
       if (s == null)
       {
           Debug.LogWarning("Sound: " + name + " not Found");
           return;
       }
       s.source.Play();
    }

    public void Stop(string name)
    {
        //Stops the sound
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not Found");
            return;
        }
        s.source.Stop();
    }
}
