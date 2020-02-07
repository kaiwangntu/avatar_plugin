using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDanMu : MonoBehaviour
{
    public RectTransform danmu;
    private string[] content = { "说“搞怪”或者戳我鼻子", "说“哭一个”或者向下拉我的嘴角", "说“笑一个”或者向上拉我的嘴角",
        "戳我眼睛", "左右滑动可以改变视角哦" };
    private RectTransform[] danmu_text;
    // Start is called before the first frame update
    void Start()
    {
        danmu_text = new RectTransform[content.Length];
        for(int i = 0; i < content.Length; i++)
        {
            danmu_text[i].gameObject.AddComponent<Text>();
            Text text = danmu_text[i].gameObject.GetComponent<Text>();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
