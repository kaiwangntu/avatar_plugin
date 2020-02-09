﻿using System.Collections;
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
        //ShowStatusBar();
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

    public void AvatarBlinkEye(string flag) {
        int i = Random.Range(0,2);
        if (i == 0)
        {
            FaceExpression.blinkEyeRight = true;
        }
        else
        {
            FaceExpression.blinkEyeRight = false;
        }
        FaceExpression.blinkTrigger = true;
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

    //public void ShowStatusBar()
    //{
    //    androidActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
    //    {
    //        androidActivity.Call<AndroidJavaObject>("getWindow").Call("clearFlags", 2048);// WindowManager.LayoutParams.FLAG_FORCE_NOT_FULLSCREEN
    //    }));

    //    //Debug.Log("show status bar");
    //}

}

