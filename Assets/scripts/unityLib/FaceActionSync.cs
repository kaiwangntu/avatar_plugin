using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceActionSync : MonoBehaviour
{

    public Button faceSync;
    public Sprite faceSyncOpen;
    public Sprite faceSyncClose;

    public static bool FACE_SYNC = false;

    private bool isFaceSync = false;
    private action_unitylib aulib;
    private ShowDanMu danmu;
    // Start is called before the first frame update
    void Start()
    {
        aulib = GameObject.Find("avatar").GetComponent<action_unitylib>();
        danmu = GameObject.Find("danmu").GetComponent<ShowDanMu>();

        faceSync.onClick.AddListener(delegate
        {
            if (isFaceSync)
            {
                isFaceSync = false;
                faceSync.GetComponent<Image>().sprite = faceSyncClose;
                FACE_SYNC = false;
                aulib.ResetBlendshape();
                AndroidCallUnityLib.androidActivity.Call("CloseCamera");
                danmu.SetIsFaceSyncDanmu(false);
                danmu.DanMuReset();
                danmu.InitFaceExpressionDanmu();
            }
            else
            {
                isFaceSync = true;
                faceSync.GetComponent<Image>().sprite = faceSyncOpen;
                FACE_SYNC = true;
                aulib.ResetBlendshape();
                AndroidCallUnityLib.androidActivity.Call("StartCamera");
                danmu.SetIsFaceSyncDanmu(true);
                danmu.DanMuReset();
                danmu.InitFaceSyncDanmu();
                danmu.OpenDanMu();
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
