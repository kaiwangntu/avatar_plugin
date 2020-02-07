using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCallUnityLib : MonoBehaviour
{
    public static AndroidJavaObject androidActivity;
    public static UnityListener unityListener;

    // Start is called before the first frame update
    void Awake()
    {
        Screen.fullScreen = false;
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        androidActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        unityListener = new UnityListener();
        androidActivity.Call("setUnityListener", unityListener);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("press escape");
            Application.Quit();
        }
    }

    public void DoFaceExpression(string flag)
    {
        FaceExpression.doFaceExpression = true;
    }

    public void AvatarCry(string flag)
    {
        FaceExpression.cryTrigger = true;
    }

    public void AvatarLaugh(string flag)
    {
        FaceExpression.laughTrigger = true;
    }

    public void AvatarFunnyFace(string flag)
    {
        FaceExpression.funnyTrigger = true;
    }

    public void StartTalking(string flag)
    {
        if (flag.Equals("true"))
        {
            AvatarSpeaking.isAvatarSpeaking = true;
            AvatarSpeaking.audioClipCount = 0;
        }
        else
        {
            AvatarSpeaking.isAvatarSpeaking = false;
            unityListener.ClearAudioClipList();
        }
    }

}

