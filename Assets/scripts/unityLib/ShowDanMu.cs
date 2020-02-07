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

    private string[] content = { "说“搞怪”或者戳我鼻子", "说“哭一个”或者向下拉我的嘴角", "说“笑一个”或者向上拉我的嘴角",
        "戳我眼睛", "左右滑动可以改变视角哦" };
    private RectTransform[] danmu_text;
    private float showWaitTime, currentWaitTime;
    private bool[] danmuMove;
    private int[] height;
    private float[] speed;
    private bool isDanmuOpen = true;

    public static bool LaughExecuted, CryExecuted, FunnyExecuted, BlinkExecuted, CamRotateExecuted;

    // Start is called before the first frame update
    void Start()
    {
        danmu_text = new RectTransform[content.Length];
        for (int i = 0; i < content.Length; i++)
        {
            GameObject danmuObj = new GameObject("danmu", typeof(RectTransform));
            danmuObj.transform.SetParent(this.gameObject.transform);
            danmu_text[i] = danmuObj.GetComponent<RectTransform>();
            danmu_text[i].sizeDelta = new Vector2(700, 100);
            danmu_text[i].pivot = new Vector2(0, 1);
            danmu_text[i].anchorMax = new Vector2(1, 1);
            danmu_text[i].anchorMin = new Vector2(1, 1);
            danmu_text[i].anchoredPosition = new Vector2(0, 0);

            danmu_text[i].gameObject.AddComponent<Text>();
            Text text = danmu_text[i].gameObject.GetComponent<Text>();
            text.text = content[i];
            text.color = new Color(1, 1, 1, 1);
            text.fontSize = Random.Range(35, 45);
            text.fontStyle = FontStyle.Normal;
            text.alignment = TextAnchor.MiddleLeft;
            text.font = font;
        }
        danmuMove = new bool[content.Length];
        height = new int[content.Length];
        speed = new float[content.Length];
        showWaitTime = Random.Range(3f, 5f);

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
            if (currentWaitTime > showWaitTime)
            {
                int danmuIndex = Random.Range(0, 5);
                //if (CheckIfDanMuShouldShow(danmuIndex) && !danmuMove[danmuIndex])
                if(!danmuMove[danmuIndex])
                {
                    speed[danmuIndex] = Random.Range(60f, 100f);
                    height[danmuIndex] = Random.Range(0, 10);
                    danmuMove[danmuIndex] = true;
                    showWaitTime = Random.Range(7f, 10f);
                    currentWaitTime = 0f;
                }
            }

            //if (!AllDanMuStop())
            //{
                currentWaitTime += Time.deltaTime;
                DanMuMove(height, speed);
            //}
        }
    }

    void DanMuMove(int[] height, float[] speed)
    {
        for(int i = 0; i < danmuMove.Length; i++) {
            if (danmuMove[i])
            {
                danmu_text[i].anchoredPosition = new Vector2(danmu_text[i].anchoredPosition.x - speed[i] * Time.deltaTime, height[i]*-13);
                if (danmu_text[i].anchoredPosition.x < -(Screen.width + danmu_text[i].sizeDelta.x))
                {
                    danmuMove[i] = false;
                    danmu_text[i].anchoredPosition = new Vector2(0, 0);
                    Debug.Log("reset");
                    showWaitTime = Random.Range(4f, 5f);
                    currentWaitTime = 0f;
                }
            }
        }
    }

    void DanMuReset()
    {
        for(int i = 0; i < danmu_text.Length; i++)
        {
            danmuMove[i] = false;
            danmu_text[i].anchoredPosition = new Vector2(0, 0);
            currentWaitTime = 0f;
            showWaitTime = 0f;
        }
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
