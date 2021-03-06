﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSpeaking : MonoBehaviour
{
    public static bool isAvatarSpeaking;
    public AudioSource audioSource;
    public static int audioClipCount = 0;
    bool stopPlaying;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAvatarSpeaking)
        {
            if (AndroidCallUnityLib.unityListener.GetAudioClipSize() > 0 && audioClipCount < AndroidCallUnityLib.unityListener.GetAudioClipSize())
            {
                
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = AndroidCallUnityLib.unityListener.GetAudioClip(audioClipCount);
                    audioSource.Play();
                    audioClipCount++;
                    //Debug.Log("sfy111 avatar speaking:" + audioClipCount);
                }
            }
            if (stopPlaying)
            {
                stopPlaying = false;
            }
        }
        else
        {
            if (!stopPlaying)
            {
                audioSource.Stop();
                audioSource.clip = null;
                AndroidCallUnityLib.unityListener.ClearAudioClipList();
                stopPlaying = true;
                Debug.Log("sfy111 avatar shutup");
            }
        }
        action_unitylib.isSpeaking = audioSource.isPlaying;
    }

}
