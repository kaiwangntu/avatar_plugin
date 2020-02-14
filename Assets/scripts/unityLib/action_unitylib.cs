using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using UnityEngine.Networking;
using System.IO;

public class action_unitylib : MonoBehaviour
{
    public SkinnedMeshRenderer m_skinnedMeshRenderer = null;
    public float zoom_factor = 0.4f;
    public float span_factor = 3.0f;
    public string talk_csv_file = "talk_1.csv";
    public string rest_csv_file = "rest_1.csv";

    private bool talk = false;
    private int rest_data_row = 0;  //current action 
    private int talk_data_row = 0;  //current action
    private float time_pass = 0;    //time passed from the last action
    private DataTable rest_au;
    private DataTable talk_au;
    private int rest_datatable_rowcount;
    private int talk_datatable_rowcount;
    private Dictionary<string, float> blendshape_dic;
    private Dictionary<string, float> headpose_dic;
    private AUtoBlendShapes au_bs;
    public static bool isSpeaking = false;
    private bool setTalkRest = false;

    private string[] AU_actions = { "AU01", "AU02", "AU04", "AU05", "AU06", "AU07", "AU09", "AU10", "AU12", "AU14", "AU15", "AU17", "AU20", "AU23", "AU25", "AU26", "AU45" };
    private string[] Eye_actions = { "AU61", "AU62", "AU63", "AU64" };
    private string[] HP_actions = { "pose_Rx", "pose_Ry", "pose_Rz" };
    private string[] G_actions = { "gaze_angle_x", "gaze_angle_y" };
    Dictionary<string, int> blendDict = new Dictionary<string, int>();
    Mesh skinnedMesh;
    int blendShapeCount;
    public HeadRoatorBone RiggedModel;

    private void Animate(Dictionary<string, float> blendshape, Dictionary<string, float> headpose)
    {
        if(m_skinnedMeshRenderer==null){
            Debug.Log("skinned mesh is null");
        }
        Dictionary<string, float> blendshape_new = au_bs.output_blendshapes(blendshape);
        RequestBlendshapes(blendshape_new);
        RiggedModel.RequestHeadRotation(headpose);
    }

    // Start is called before the first frame update
    void Start()
    {
        //read au data from csv files
        StartCoroutine(OpenCSV_android(Application.streamingAssetsPath + "/"));

        //convert AU to blendshape
        au_bs = new AUtoBlendShapes();
        blendshape_dic = new Dictionary<string, float>();
        headpose_dic = new Dictionary<string, float>();
        foreach (string bs_elem in HP_actions)
        {
            headpose_dic.Add(bs_elem, 0);
        }
        foreach (string au_elem in AU_actions)
        {
            blendshape_dic.Add(au_elem, 0);
        }
        foreach (string eye_elem in Eye_actions)
        {
            blendshape_dic.Add(eye_elem, 0);
        }
        Debug.Log("read actions!");

        skinnedMesh = m_skinnedMeshRenderer.sharedMesh;
        // create dict of all blendshapes this skinnedMesh has
        blendShapeCount = skinnedMesh.blendShapeCount;
        for (int i = 0; i < blendShapeCount; i++)
        {
            string expression = skinnedMesh.GetBlendShapeName(i);
            //Debug.Log(expression);
            blendDict.Add(expression, i);
        }
        
        AndroidCallUnityLib.androidActivity.Call("UnityLoadComplete");
    }

	void Update()
	{
        if (isSpeaking && !setTalkRest)
        {
            TalkShow();
            setTalkRest = true;//true: current state is talk
            //LOG.WriteLog("开始说话！");
        }
        else if (!isSpeaking && setTalkRest)
        {
            Silence();
            setTalkRest = false;//false: current state is rest
            //LOG.WriteLog("说话完毕！" );
        }

        if (rest_au != null)
        {
            show();
        }
    }

	IEnumerator OpenCSV_android(string file)
    {
        
        Debug.Log("streamingassets:" + file);
        String restPath = file + rest_csv_file;
        String talkPath = file + talk_csv_file;

        //rest
        UnityWebRequest www = UnityWebRequest.Get(restPath); 
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("www error:"+www.error);
        }
        else
        {
            Debug.Log("www:"+www.downloadHandler.text); // this log is returning the requested data. 
            rest_au = GetFromUWR(www.downloadHandler.text);
            rest_datatable_rowcount = rest_au.Rows.Count;
        }

        //talk
        UnityWebRequest www2 = UnityWebRequest.Get(talkPath);
        yield return www2.SendWebRequest();
        if (www2.isNetworkError || www2.isHttpError)
        {
            Debug.Log("www2 error:" + www2.error);
        }
        else
        {
            Debug.Log("www2:" + www2.downloadHandler.text); // this log is returning the requested data. 
            talk_au = GetFromUWR(www2.downloadHandler.text);
            talk_datatable_rowcount = talk_au.Rows.Count;
        }
    }

    DataTable GetFromUWR(String uwrData){
        DataTable dt = new DataTable();
        string[] str = uwrData.Split('\n');
        string head = str[0];
        string[] headNames = head.Split(',');
        for (int i = 0; i < headNames.Length; i++)
        {
            headNames[i] = headNames[i].Replace("_r", "");
            dt.Columns.Add(headNames[i], typeof(string));
        }
        for (int i = 1; i < str.Length; i++)
        {
            string lineStr = str[i];
            if (lineStr == null || lineStr.Length == 0)
                continue;
            string[] values = lineStr.Split(',');
            DataRow dr = dt.NewRow();
            for (int j = 0; j < values.Length; j++)
            {
                dr[j] = values[j];
            }
            dt.Rows.Add(dr);
        }
        return dt;
    }

    //private void Update()
    public void show()
    {
        time_pass += Time.deltaTime;
        if (talk)
        {
            float time_stamp = Convert.ToSingle(talk_au.Rows[talk_data_row]["timestamp"])*2.0f;
            //ebug.Log("time_stamp="+ time_stamp.ToString());
            if (time_pass > time_stamp)
            {
                //Debug.Log("time_pass=" + time_pass.ToString());
                //read action data in datatable and animate mesh
                float confidence = Convert.ToSingle(talk_au.Rows[talk_data_row]["confidence"]);
                if (confidence > 0.7)
                {
                    DataRow row_data = talk_au.Rows[talk_data_row];
                    //Debug.Log("talk data row: " + talk_data_row.ToString());
                    foreach (string au_elem in AU_actions)
                    {
                        float mvalue = Convert.ToSingle(row_data[au_elem]);
                        blendshape_dic[au_elem] = mvalue * 0.3f;
                    }
                    foreach (string hp_elem in HP_actions)
                    {
                        float mvalue = Convert.ToSingle(row_data[hp_elem]);
                        headpose_dic[hp_elem] = mvalue * 0.3f;
                    }
                    float[] g_values = { Convert.ToSingle(row_data[G_actions[0]]) * 0.3f, Convert.ToSingle(row_data[G_actions[1]]) * 0.3f };
                    if (g_values[0] < 0)
                        blendshape_dic["AU61"] = Math.Min(g_values[0] * -1, 1.0f);
                    else
                        blendshape_dic["AU62"] = Math.Min(g_values[0], 1.0f);
                    if (g_values[1] >= 0)
                        blendshape_dic["AU63"] = Math.Min(g_values[1], 1.0f);
                    else
                        blendshape_dic["AU64"] = Math.Min(g_values[1] * -1, 1.0f);

                    Animate(blendshape_dic, headpose_dic);
                }

                talk_data_row += 1;
                if (talk_data_row >= talk_datatable_rowcount)
                {
                    //talk = true;
                    rest_data_row = 0;
                    talk_data_row = 0;
                    time_pass = 0.0f;
                }
            }
        }
        else
        {
            //LOG.WriteLog("帧序列" + rest_data_row.ToString());
            float time_stamp = Convert.ToSingle(rest_au.Rows[rest_data_row]["timestamp"])* 1.5f;
            //Debug.Log("time_stamp=" + time_stamp.ToString());
            if (time_pass > time_stamp)
            {
                //read action data in datatable and animate mesh
                float confidence = Convert.ToSingle(rest_au.Rows[rest_data_row]["confidence"]);
                if (confidence > 0.7)
                {
                    DataRow row_data = rest_au.Rows[rest_data_row];
                    foreach (string au_elem in AU_actions)
                    {
                        //Debug.Log(au_elem);
                        float mvalue = Convert.ToSingle(row_data[au_elem]) * 0.4f;
                        //Debug.Log("au_elem=" + mvalue.ToString());
                        blendshape_dic[au_elem] = mvalue;
                    }
                    foreach (string hp_elem in HP_actions)
                    {
                        float mvalue = Convert.ToSingle(row_data[hp_elem]);
                        headpose_dic[hp_elem] = mvalue;
                    }
                    float[] g_values = { Convert.ToSingle(row_data[G_actions[0]]) * 0.4f, Convert.ToSingle(row_data[G_actions[1]]) * 0.4f };
                    if (g_values[0] < 0)
                        blendshape_dic["AU61"] = Math.Min(g_values[0] * -1, 1.0f);
                    else
                        blendshape_dic["AU62"] = Math.Min(g_values[0], 1.0f);
                    if (g_values[1] >= 0)
                        blendshape_dic["AU63"] = Math.Min(g_values[1], 1.0f);
                    else
                        blendshape_dic["AU64"] = Math.Min(g_values[1] * -1, 1.0f);

                    Animate(blendshape_dic, headpose_dic);
                }
                rest_data_row += 1;
                if (rest_data_row >= rest_datatable_rowcount)
                {
                    //talk = false;
                    rest_data_row = 0;
                    talk_data_row = 0;
                    time_pass = 0.0f;
                }
            }
        }
    }

    public void TalkShow()
    {
        talk = true;
        rest_data_row = 0;
        talk_data_row = 0;
        time_pass = 0.0f;
        //Debug.Log(talk.ToString());
    }

    public void Silence()
    {
        talk = false;
        rest_data_row = 0;
        talk_data_row = 0;
        time_pass = 0.0f;
        //Debug.Log(talk.ToString());
    }

    public void RequestBlendshapes(Dictionary<string, float> blendshape)
    {
        // animate character with received Blend Shape values per Blend Shape, pass on new value to character
        foreach (KeyValuePair<string, float> pair in blendshape)
        {
            //Debug.Log(pair);  // Debug.Log verrrryy slow, don't use in production (>100x slower)
            float blend_val = pair.Value;
            if (blend_val > 1.0f) blend_val = 1.0f;
            //Debug.Log("sfy face expression:"+FaceExpression.doFaceExpression);
            if (!FaceActionSync.FACE_SYNC)
            {
                if (FaceExpression.doFaceExpression)
                {
                    //if (blendDict[pair.Key] == 48 || blendDict[pair.Key] == 49)
                    //{
                    //    m_skinnedMeshRenderer.SetBlendShapeWeight(blendDict[pair.Key], blend_val * 100.0f);
                    //}
                }
                else
                {
                    m_skinnedMeshRenderer.SetBlendShapeWeight(blendDict[pair.Key], blend_val * 100.0f);
                }
            }
        }

        //yield return null;
    }

    public void ResetBlendshape()
    {
        for(int i=0;i< blendShapeCount; i++)
        {
            m_skinnedMeshRenderer.SetBlendShapeWeight(i, 0f);
        }
    }

}
