﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHintSentence : MonoBehaviour
{
    public Transform text3d;
    public RectTransform text2d;

    public static bool RefreshCaption;

    string[] content_3d = new string[] { "易感人群\n传播途径\n口罩种类\n居家隔离\n怀疑感染\n疫情在线咨询\n学生防控\n老年人防控\n办公室防控\n确诊患者车次\n确诊患者小区",
                                         "北京天气\n美元汇率\n联通股价\n五加五等于几\n今天几号\n今天农历几号\n现在几点了\n静夜思的作者\n背诵静夜思\n讲一个笑话"};
    string[] content_2d = new string[] { };

    TextMesh t_3d;
    Text t_2d;

    int contentIndex3d = 0, contentIndex2d = 0;

    float showTime3d = 3f, showTime2d = 3f;
    float currentTime3d = 0f, currentTime2d = 0f;
    float alpha3d, alpha2d, alphaSpeed3d = 2f, speed2d = 120f;
    int characterWidth = 50;//当前字号38下，每个字占位为50

    bool isShow_3d=true, isShow_2d=true, turn2Trans_3d, turn2Trans_2d;

    Color color_3d, color_2d;
    // Start is called before the first frame update
    void Start()
    {
        t_3d = text3d.GetComponent<TextMesh>();
        t_3d.text = content_3d[contentIndex3d];
        color_3d = t_3d.color;
        alpha3d = color_3d.a;

        t_2d = text2d.GetComponent<Text>();
        t_2d.text = "";
        color_2d = t_2d.color;//#636363
        alpha2d = color_2d.a;
    }

    // Update is called once per frame
    void Update()
    {
        //Show3DText();
        Show2DText();
        if (RefreshCaption)
        {
            RefreshCaption = false;
            RefreshText2d();
        }
    }

    void Show3DText()
    {
        if (currentTime3d > showTime3d)//展示时间达到，切换文字
        {
            isShow_3d = false;
            turn2Trans_3d = true;
            currentTime3d = 0f;
        }
        if (isShow_3d)
        {
            currentTime3d += Time.deltaTime;
        }
        else
        {
            if (turn2Trans_3d)
            {
                alpha3d -= alphaSpeed3d * Time.deltaTime;
                t_3d.color = new Color(color_3d.r, color_3d.g, color_3d.b, alpha3d);
                if (alpha3d <= 0)
                {
                    turn2Trans_3d = false;
                    contentIndex3d++;
                    if (contentIndex3d >= content_3d.Length)
                    { 
                        contentIndex3d = 0;
                    }
                    t_3d.text = content_3d[contentIndex3d];
                }
            }
            else
            {
                alpha3d += alphaSpeed3d * Time.deltaTime;
                t_3d.color = new Color(color_3d.r, color_3d.g, color_3d.b, alpha3d);
                if (alpha3d > 0.95f)
                {
                    currentTime3d = 0f;
                    isShow_3d = true;
                }
            }
        }
    }

    void Show2DText()
    {
        text2d.anchoredPosition = new Vector2(text2d.anchoredPosition.x - speed2d * Time.deltaTime, text2d.anchoredPosition.y);
        if (text2d.anchoredPosition.x < -(Screen.width + 2 * text2d.sizeDelta.x))
        {
            t_2d.text = "";
            text2d.anchoredPosition = new Vector2(0, 0);
        }
    }

    void RefreshText2d()
    {
        string str = AndroidCallUnityLib.unityListener.getCaptionContent();
        text2d.sizeDelta = new Vector2(characterWidth * str.Length, 50);
        t_2d.text = str;
        if (AndroidCallUnityLib.unityListener.getCaptionType() == 0)
        {
            t_2d.color = color_2d;
        }
        else
        {
            t_2d.color = new Color(0.78f, 0f, 0f, 1f);
        }
        text2d.anchoredPosition = new Vector2(0, 0);
    }
}
