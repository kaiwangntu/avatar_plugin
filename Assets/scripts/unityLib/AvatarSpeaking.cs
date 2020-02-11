using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSpeaking : MonoBehaviour
{
    public static bool isAvatarSpeaking;
    public AudioSource audioSource;
    public static int audioClipCount = 0;
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
                    //Debug.Log("sfy111 avatar speaking:"+audioClipCount);
                }
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.clip = null;
                audioSource.Stop();
                //Debug.Log("sfy111 avatar shutup");
            }
        }
        action_unitylib.isSpeaking = audioSource.isPlaying;
    }
}
