using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;

public class LOG
{
    private static FileStream fs;
    private static StreamWriter sw;

    public static void Init()
    {
        fs = new FileStream("Assets/log.txt", FileMode.OpenOrCreate);
        sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("gb2312"));
        if (sw==null)
        {
            Debug.Log("StreamWriter for log created failed!");
        }
        DateTime currentTime = System.DateTime.Now;
        string strY = currentTime.ToString("f");
        sw.WriteLine(strY);
    }

    public static void WriteLog(string str)
    {
        sw.WriteLine(str);
    }

    public static void Uninit()
    {  
        sw.Close();
        fs.Close();
    }
}
