using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidCallUnityLib : MonoBehaviour
{
    public static AndroidJavaObject androidActivity;
    public static UnityListener unityListener;

    public Button modeChange;

    private int mode = 0;

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

    void Start()
    {
        modeChange.onClick.AddListener(delegate
        {
            mode++;
            if (mode==1) //健康自测
            {
                modeChange.transform.GetChild(0).GetComponent<Text>().text = "健康自测";
                modeChange.transform.GetChild(0).GetComponent<Text>().color = new Color(0, 1, 0.1f, 1);
                androidActivity.Call("testingHealth", 1);
            }
            else if(mode==2) //疫情知识考试
            {
                modeChange.transform.GetChild(0).GetComponent<Text>().text = "疫情考试";
                modeChange.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 0, 0, 1);
                androidActivity.Call("testingHealth", 2);
            }else if (mode == 3)
            {
                modeChange.transform.GetChild(0).GetComponent<Text>().text = "普通问答";
                modeChange.transform.GetChild(0).GetComponent<Text>().color = new Color(1, 1, 1, 1);
                androidActivity.Call("testingHealth", 0);
                mode = 0;
            }
        });
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

    public void DoHumanAction(string flag)
    {
        if (flag.Equals("face"))
        {
            FaceExpression.doFaceExpression = true;
        }
        else if(flag.Equals("body"))
        {
            HeadRoatorBone.doBodyAction = true;
        }
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

    public void AvatarHeadNod(string flag)
    {
        HeadRoatorBone.nodTrigger = true;
    }

    public void StartTalking(string flag)
    {
        if (flag.Equals("true"))
        {
            AvatarSpeaking.isAvatarSpeaking = true;
            AvatarSpeaking.audioClipCount = 0;
            Debug.Log("sfy111 start talking");
        }
        else
        {
            AvatarSpeaking.isAvatarSpeaking = false;
            
            Debug.Log("sfy111 stop talking");
        }
    }

    public void HumanActionSynchronize(string flag)
    {
        if (!FaceExpression.doFaceExpression)
        {
            switch (flag)
            {
                case "eye_blink":
                    FaceExpression.blinkSyncTrigger = true;
                    FaceExpression.doFaceExpression = true;
                    break;
                case "mouth_open":
                    FaceExpression.mouthOpenSyncTrigger = true;
                    FaceExpression.doFaceExpression = true;
                    break;
                case "brow_jump":
                    FaceExpression.browJumpSyncTrigger = true;
                    FaceExpression.doFaceExpression = true;
                    break;
            }
        }
        if (!HeadRoatorBone.doBodyAction)
        {
            switch (flag)
            {
                case "head_yaw":
                    HeadRoatorBone.shakeSyncTrigger = true;
                    HeadRoatorBone.doBodyAction = true;
                    break;
                case "head_pitch":
                    HeadRoatorBone.nodSyncTrigger = true;
                    HeadRoatorBone.doBodyAction = true;
                    break;
            }
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

