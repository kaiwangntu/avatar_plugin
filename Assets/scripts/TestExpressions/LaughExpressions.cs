using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaughExpressions : MonoBehaviour
{
    public SkinnedMeshRenderer m_skinnedMeshRenderer = null;
    Dictionary<string, int> blendDict = new Dictionary<string, int>();
    Mesh skinnedMesh;
    int blendShapeCount;

    Dictionary<string, float> laughBlend;
    float[] value, end;
    Dictionary<string, int> laughStepIndex;
    int[] laughStepCount;

    float smile_v, jaw_v, chin_v, eye_v, cheek_v, brow_v;
    bool doonce, step1, step2, step3, step4, step5;
    bool smile;

    // Start is called before the first frame update
    void Start()
    {
        skinnedMesh = m_skinnedMeshRenderer.sharedMesh;
        // create dict of all blendshapes this skinnedMesh has
        blendShapeCount = skinnedMesh.blendShapeCount;
        for (int i = 0; i < blendShapeCount; i++)
        {
            string expression = skinnedMesh.GetBlendShapeName(i);
            //Debug.Log(expression);
            blendDict.Add(expression, i);
        }

        laughBlend = new Dictionary<string, float>
        {
            {"MouthSmile_R", 0f},
            {"MouthSmile_L", 0f},
            {"JawOpen", 0f},
            {"ChinUpperRaise",0f},
            {"CheekSquint_R", 0f},
            {"CheekSquint_L",0f},
            {"BrowsU_R",0f},
            {"BrowsU_L",0f},
            {"EyeSquint_R",0f},
            {"EyeSquint_L",0f}
        };
        //DebugBlendShapeIndex(laughBlend);
        smile = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (40.0 - smile_v > 0.5f || 70f - chin_v > 0.5f || 50f - cheek_v > 0.5f || 100f - eye_v > 0.5f || 20f - brow_v > 0.5f)
        {

        }
        else
        {
            smile = false;
        }
        if (smile)
        {
            smile_v = Mathf.Lerp(smile_v, 40.0f, 1.5f * Time.deltaTime);
            chin_v = Mathf.Lerp(chin_v, 70f, 1.5f * Time.deltaTime);
            cheek_v = Mathf.Lerp(cheek_v, 50f, 1.5f * Time.deltaTime);
            eye_v = Mathf.Lerp(eye_v, 100f, Time.deltaTime);
            brow_v = Mathf.Lerp(brow_v, 20f, 1.5f * Time.deltaTime);
        }
        else
        {
            smile_v = Mathf.Lerp(smile_v, 0.0f, 1.5f * Time.deltaTime);
            chin_v = Mathf.Lerp(chin_v, 0f, 1.5f * Time.deltaTime);
            cheek_v = Mathf.Lerp(cheek_v, 0f, 1.5f * Time.deltaTime);
            eye_v = Mathf.Lerp(eye_v, 0f, Time.deltaTime);
            brow_v = Mathf.Lerp(brow_v, 0f, 1.5f * Time.deltaTime);
        }

        m_skinnedMeshRenderer.SetBlendShapeWeight(16, smile_v);
        m_skinnedMeshRenderer.SetBlendShapeWeight(17, smile_v);
        m_skinnedMeshRenderer.SetBlendShapeWeight(51, chin_v);
        m_skinnedMeshRenderer.SetBlendShapeWeight(54, cheek_v);
        m_skinnedMeshRenderer.SetBlendShapeWeight(53, cheek_v);
        m_skinnedMeshRenderer.SetBlendShapeWeight(44, eye_v);
        m_skinnedMeshRenderer.SetBlendShapeWeight(45, eye_v);
        m_skinnedMeshRenderer.SetBlendShapeWeight(59, brow_v);
        m_skinnedMeshRenderer.SetBlendShapeWeight(60, brow_v);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            smile = true;
        }
    }

    void DebugBlendShapeIndex(Dictionary<string,float> blendshape)
    {
        foreach (KeyValuePair<string, float> pair in blendshape)
        {
            Debug.Log(pair.Key+":"+blendDict[pair.Key]);  // Debug.Log verrrryy slow, don't use in production (>100x slower)
           
        }
    }
}
