using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VideoPlayer = UnityEngine.Video.VideoPlayer;

public class VideoPlayer : MonoBehaviour
{
    [SerializeField] private string videoFileName;

    private void Awake()
    {
        PlayVideo();
    }

    public void PlayVideo()
    {
        UnityEngine.Video.VideoPlayer videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
        
        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
            Debug.Log(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
}
