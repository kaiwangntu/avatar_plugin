using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunnyExpressions : MonoBehaviour
{
    public SkinnedMeshRenderer m_skinnedMeshRenderer = null;
    Dictionary<string, int> blendDict = new Dictionary<string, int>();
    Mesh skinnedMesh;
    int blendShapeCount;

    Dictionary<string, float> funnyBlend;
    float[] value, end;
    Dictionary<string, int> laughStepIndex;
    int[] laughStepCount;

    float browU_v, browD_v, cheek_v, mouth_v;
    bool doonce, step1, step2, step3, step4, step5;
    bool smile, brow_down;
    int browsCount = 0;

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

        funnyBlend = new Dictionary<string, float>
        {
            {"BrowsU_L", 0f},//57
            {"BrowsD_R", 0f},//59
            {"CheekSquint_R", 0f},//53
            {"MouthRight",0f}//18

        };
        DebugBlendShapeIndex(funnyBlend);

        step1 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (step1)
        {
            if (brow_down)
            {
                browU_v = Mathf.Lerp(browU_v, 0.0f, 10f * Time.deltaTime);
                browD_v = Mathf.Lerp(browD_v, 0f, 10f * Time.deltaTime);
                mouth_v = Mathf.Lerp(mouth_v, 0f, 10f * Time.deltaTime);
                cheek_v = Mathf.Lerp(cheek_v, 0f, 10f * Time.deltaTime);
            }
            else
            {
                browU_v = Mathf.Lerp(browU_v, 100.0f, 10f * Time.deltaTime);
                browD_v = Mathf.Lerp(browD_v, 60f, 10f * Time.deltaTime);
                mouth_v = Mathf.Lerp(mouth_v, 50f, 10f * Time.deltaTime);
                cheek_v = Mathf.Lerp(cheek_v, 50f, 10f * Time.deltaTime);
            }
            m_skinnedMeshRenderer.SetBlendShapeWeight(57, browU_v);
            m_skinnedMeshRenderer.SetBlendShapeWeight(59, browD_v);
            m_skinnedMeshRenderer.SetBlendShapeWeight(53, cheek_v);
            m_skinnedMeshRenderer.SetBlendShapeWeight(18, mouth_v);
            if (100f - browU_v < 0.5f)
            {
                brow_down = true;
            }
            if (browU_v - 0f < 0.5f)
            {
                brow_down = false;
                step1 = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            step1 = true;
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
