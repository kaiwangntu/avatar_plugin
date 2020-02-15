using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDanMu : MonoBehaviour
{
    public Font font;
    public Button openDanmu;
    public Sprite danmuOpen;
    public Sprite danmuClose;
    public Transform danmuParent;

    private string[] content = { "说“搞怪”或者戳我鼻子", "说“哭一个”或者向下拉我的嘴角", "说“笑一个”或者向上拉我的嘴角",
        "戳我眼睛", "左右滑动可以改变视角哦", "说“点头”或者戳我额头" };
    private string[] faceSyncContent = {"前置相机已打开","眨眼试试","点点头吧","摇摇头","挑眉试试","张张嘴", "前置相机已打开", "眨眼试试", "点点头吧", "摇摇头", "挑眉试试", "张张嘴"};
    private RectTransform[] danmu_text;
    private float showWaitTime, currentWaitTime;
    private bool[] danmuMove;
    private int[] height;
    private float[] speed;
    private bool isDanmuOpen = true;
    private int characterWidth = 50;
    private bool isFaceSyncDanmu = false;

    public static bool LaughExecuted, CryExecuted, FunnyExecuted, BlinkExecuted, CamRotateExecuted;

    // Start is called before the first frame update
    void Start()
    {
        InitFaceExpressionDanmu();

        openDanmu.onClick.AddListener(delegate
        {
            if (isDanmuOpen)
            {
                isDanmuOpen = false;
                openDanmu.GetComponent<Image>().sprite = danmuClose;
                DanMuReset();
            }
            else
            {
                isDanmuOpen = true;
                openDanmu.GetComponent<Image>().sprite = danmuOpen;
                showWaitTime = Random.Range(0.5f, 2f);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (isDanmuOpen)
        {
            if (!isFaceSyncDanmu)
            {
                RandomDanmu(7f, 10f);
            }
            else
            {
                RandomDanmuFaceSync(2.5f, 4f);
            }
            

            //if (!AllDanMuStop())
            //{
            currentWaitTime += Time.deltaTime;
                DanMuMove(height, speed);
            //}
        }
    }

    public void SetIsFaceSyncDanmu(bool flag)
    {
        isFaceSyncDanmu = flag;
    }

    public void InitFaceExpressionDanmu()
    {
        danmu_text = new RectTransform[content.Length];
        for (int i = 0; i < content.Length; i++)
        {
            GameObject danmuObj = new GameObject("danmu", typeof(RectTransform));
            danmuObj.transform.SetParent(danmuParent);
            danmu_text[i] = danmuObj.GetComponent<RectTransform>();
            danmu_text[i].sizeDelta = new Vector2(content[i].Length * characterWidth, 100);
            danmu_text[i].pivot = new Vector2(0, 1);
            danmu_text[i].anchorMax = new Vector2(1, 1);
            danmu_text[i].anchorMin = new Vector2(1, 1);
            danmu_text[i].anchoredPosition = new Vector2(0, 0);

            danmu_text[i].gameObject.AddComponent<Text>();
            Text text = danmu_text[i].gameObject.GetComponent<Text>();
            text.text = content[i];
            text.color = new Color(1, 0.9f, 0, 1);
            text.fontSize = Random.Range(35, 45);
            text.fontStyle = FontStyle.Normal;
            text.alignment = TextAnchor.MiddleLeft;
            text.font = font;
        }
        danmuMove = new bool[content.Length];
        height = new int[content.Length];
        speed = new float[content.Length];
        showWaitTime = Random.Range(1f, 3f);
    }

    public void InitFaceSyncDanmu()
    {
        danmu_text = new RectTransform[faceSyncContent.Length];
        for (int i = 0; i < faceSyncContent.Length; i++)
        {
            GameObject danmuObj = new GameObject("danmu", typeof(RectTransform));
            danmuObj.transform.SetParent(danmuParent);
            danmu_text[i] = danmuObj.GetComponent<RectTransform>();
            danmu_text[i].sizeDelta = new Vector2(faceSyncContent[i].Length * characterWidth, 100);
            danmu_text[i].pivot = new Vector2(0, 1);
            danmu_text[i].anchorMax = new Vector2(1, 1);
            danmu_text[i].anchorMin = new Vector2(1, 1);
            danmu_text[i].anchoredPosition = new Vector2(0, 0);

            danmu_text[i].gameObject.AddComponent<Text>();
            Text text = danmu_text[i].gameObject.GetComponent<Text>();
            text.text = faceSyncContent[i];
            text.color = new Color(1, 0.9f, 0, 1);
            text.fontSize = Random.Range(35, 45);
            text.fontStyle = FontStyle.Normal;
            text.alignment = TextAnchor.MiddleLeft;
            text.font = font;
        }
        danmuMove = new bool[faceSyncContent.Length];
        height = new int[faceSyncContent.Length];
        speed = new float[faceSyncContent.Length];
        showWaitTime = 0f;
    }

    void RandomDanmu(float minWaitTime, float maxWaitTime)
    {
        if (currentWaitTime > showWaitTime)
        {
            int danmuIndex = Random.Range(0, content.Length);
            //if (CheckIfDanMuShouldShow(danmuIndex) && !danmuMove[danmuIndex])
            if (!danmuMove[danmuIndex])
            {
                speed[danmuIndex] = Random.Range(100f, 130f);
                height[danmuIndex] = Random.Range(2, 10);
                danmuMove[danmuIndex] = true;
                showWaitTime = Random.Range(minWaitTime, maxWaitTime);
                currentWaitTime = 0f;
            }
        }
    }

    void RandomDanmuFaceSync(float minWaitTime, float maxWaitTime)
    {
        if (currentWaitTime > showWaitTime)
        {
            int danmuIndex = 0;
            for(int i = 0; i < faceSyncContent.Length; i++)
            {
                if (!danmuMove[i])
                {
                    danmuIndex = i;
                    break;
                }
            }
            //if (CheckIfDanMuShouldShow(danmuIndex) && !danmuMove[danmuIndex])
            if (!danmuMove[danmuIndex])
            {
                speed[danmuIndex] = Random.Range(100f, 130f);
                height[danmuIndex] = Random.Range(2, 10);
                danmuMove[danmuIndex] = true;
                showWaitTime = Random.Range(minWaitTime, maxWaitTime);
                currentWaitTime = 0f;
            }
        }
    }

    void DanMuMove(int[] height, float[] speed)
    {
        for(int i = 0; i < danmuMove.Length; i++) {
            if (danmuMove[i])
            {
                danmu_text[i].anchoredPosition = new Vector2(danmu_text[i].anchoredPosition.x - speed[i] * Time.deltaTime, height[i]*-12);
                if (danmu_text[i].anchoredPosition.x < -(Screen.width + 2 * danmu_text[i].sizeDelta.x))
                {
                    danmuMove[i] = false;
                    danmu_text[i].anchoredPosition = new Vector2(0, 0);
                    //Debug.Log("reset");
                    showWaitTime = Random.Range(4f, 5f);
                    currentWaitTime = 0f;
                }
            }
        }
    }

    public void DanMuReset()
    {
        for(int i = 0; i < danmu_text.Length; i++)
        {
            danmuMove[i] = false;
            danmu_text[i].anchoredPosition = new Vector2(0, 0);
            currentWaitTime = 0f;
            showWaitTime = 0f;
        }
    }

    public void OpenDanMu()
    {
        isDanmuOpen = true;
        openDanmu.GetComponent<Image>().sprite = danmuOpen;
    }

    bool CheckIfDanMuShouldShow(int danmuIndex)
    {
        if (LaughExecuted && danmuIndex == 2)
        {
            return false;
        }
        else if (CryExecuted && danmuIndex == 1)
        {
            return false;
        }
        else if (FunnyExecuted && danmuIndex == 0)
        {
            return false;
        }
        else if (BlinkExecuted && danmuIndex == 3)
        {
            return false;
        }
        else if (CamRotateExecuted && danmuIndex ==4)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool AllDanMuStop()
    {
        if (LaughExecuted && CryExecuted && FunnyExecuted && BlinkExecuted && CamRotateExecuted && !CheckAllDanMuIsMoving())
        {
            currentWaitTime = 0f;
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckAllDanMuIsMoving()
    {
        for(int i = 0; i < danmuMove.Length; i++)
        {
            if (danmuMove[i])
            {
                return true;
            }
        }
        return false;
    }
}
