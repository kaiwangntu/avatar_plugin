using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json.Linq;  // JSON reader; https://assetstore.unity.com/packages/tools/input-management/json-net-for-unity-11347
using System.Linq;
//using Newtonsoft.Json;
//using System.Web.Script.Serialization;
using System.IO;
using System;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

//change AU values from OpenFace to Blend Shape (Shape Key) values understood by Unity / Blender / Unreal
//for characters created in Blender + Manual Bastioni addon
class AUtoBlendShapes
{

    private Dictionary<string, float> blendshape_dict;
    private Dictionary<string, Dictionary<string, float>> au_dict;
    Dictionary<string, float> blendshape_vals;
    int frame_tracker;

    public AUtoBlendShapes()
    {
#if UNITY_EDITOR_WIN
        //load_json("Assets/openface/au_json");

        //frame tracker for index in dataframe
        //frame_tracker = 0;

        //AU to blendshapes
        //json2dic("Assets/openface/blendshapes_MB.json");
#else
        au_dict = AndroidCallUnityLib.unityListener.GetAU_dict();
        //frame tracker for index in dataframe
        frame_tracker = 0;

        //AU to blendshapes
        blendshape_dict = AndroidCallUnityLib.unityListener.GetBlendshape_dict();
#endif
    }

    // receive AU values and change to blendshapes
    public void calc_blendshapes(Dictionary<string, float> au_values)
    {
        // get a clean blendshape dict,reset dict to default 0
        blendshape_vals = new Dictionary<string, float>();
        foreach (KeyValuePair<string, float> pair in blendshape_dict)
        {
            blendshape_vals.Add(pair.Key,0);
        }

        
        //loop over AU values from OpenFace data frame
        foreach (KeyValuePair<string, float> pair in au_values)
        {
            if (au_dict.ContainsKey(pair.Key))
            {
                if (pair.Value > 0.001)
                {
                    Dictionary<string, float> au_dict_elem = au_dict[pair.Key];
                    foreach (KeyValuePair<string, float> e in au_dict_elem)
                    {
                        //Debug.Log("au_dict_elem key : " + e.Key);
                        //multiply AU value with au_dict to get blendshape values and add blendshape values to total blendshape_dict
                        blendshape_vals[e.Key] += (float)(Math.Round(e.Value * pair.Value, 5));
                    }
                }
            }
            else 
            {
                Console.Write("No json file found for " + pair.Key);
            }
        }
        
    }

    public Dictionary<string, float> output_blendshapes(Dictionary<string, float> au_values)
    {
        Console.Write("Frame: " + frame_tracker.ToString());
        calc_blendshapes(au_values);
        frame_tracker += 1;
        return blendshape_vals;
    }


}