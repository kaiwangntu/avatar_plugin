using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static TTSUtil;

public class UnityListener : AndroidJavaProxy
{
    private Dictionary<string, float> blendshape_dict;
    private Dictionary<string, Dictionary<string, float>> au_dict;
    private List<AudioClip> audioClipList = new List<AudioClip>();

    public UnityListener() : base("com.unicom.unitylibinterface.UnitylibListener")
    {
    }

    public void SendActionUnit(int requestCode, int resultCode, AndroidJavaObject data)
    {
        Debug.Log("test unity lib listener");
        au_dict = new Dictionary<string, Dictionary<string, float>>();

        AndroidJavaObject[] au = data.Call<AndroidJavaObject[]>("getActionUnit");
        Dictionary<string, float>[] bs_dic = new Dictionary<string, float>[resultCode];
        
        int bs_dic_count = 0;
        bs_dic[0] = new Dictionary<string, float>();
        string prev = au[0].Call<string>("getAuIndex");
        for(int i = 0; i < au.Length; i++)
        {
            string index = au[i].Call<string>("getAuIndex");
            if (!index.Equals(prev))
            {
                au_dict.Add(prev, bs_dic[bs_dic_count]);
                prev = index;
                bs_dic_count++;
                bs_dic[bs_dic_count] = new Dictionary<string, float>();
            }
            string name = au[i].Call<string>("getAuName");
            float value = au[i].Call<float>("getAuValue");
            bs_dic[bs_dic_count].Add(name, (float)Math.Round(value, 5));
        }
    }

    public void SendBlendshape(int requestCode, int resultCode, AndroidJavaObject data)
    {
        blendshape_dict = new Dictionary<string, float>();
        AndroidJavaObject[] blendshape = data.Call<AndroidJavaObject[]>("getActionUnit");
        for (int i = 0; i < blendshape.Length; i++)
        {
            string name = blendshape[i].Call<string>("getAuName");
            float value = blendshape[i].Call<float>("getAuValue");
            blendshape_dict.Add(name, value);
        }
    }

    public void SendByteData(int requestCode, int resultCode, AndroidJavaObject data)
    {
        byte[] buffer = data.Call<byte[]>("getByteData");
        Debug.Log("unity onActivityResult data:" + buffer.Length);

        MemoryStream memoryStream = new MemoryStream();
        memoryStream.Write(new byte[44], 0, 44);
        memoryStream.Write(buffer, 0, buffer.Length);
        WAVE_Header wave_Header = getWave_Header((int)memoryStream.Length);
        byte[] array2 = StructToBytes(wave_Header);
        memoryStream.Position = 0L;//set pointer to start
        memoryStream.Write(array2, 0, array2.Length);//存储结构体的字节数组
        memoryStream.Position = 0L;//set pointer to start 

        Debug.Log("unity onActivityResult buffer:" + memoryStream.GetBuffer().Length);

        AudioClip ac = FromWavData(memoryStream.GetBuffer());
        audioClipList.Add(ac);
    }

    public Dictionary<string, float> GetBlendshape_dict()
    {
        return blendshape_dict;
    }

    public Dictionary<string, Dictionary<string, float>> GetAU_dict()
    {
        return au_dict;
    }

    public List<AudioClip> getAudioClipList()
    {
        return audioClipList;
    }

    public AudioClip GetAudioClip(int i)
    {
        return audioClipList[i];
    }

    public int GetAudioClipSize() {
        return audioClipList.Count;
    }

    public void ClearAudioClipList()
    {
        audioClipList.Clear();
    }
}
