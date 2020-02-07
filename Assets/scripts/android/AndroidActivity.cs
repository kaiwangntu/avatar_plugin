using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidActivity : MonoBehaviour
{
    public static AndroidJavaObject androidActivity;

    void Awake()
    {
        //Initiate Android
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        androidActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        androidActivity.Call("GetDataPath", Application.persistentDataPath + "/tts.wav");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
