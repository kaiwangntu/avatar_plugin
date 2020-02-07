using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Expression{
    public int index;
    public float[] value;
    public float[] speed;
    public Expression(int index, float[] value, float[] speed)
    {
        this.index = index;
        this.value = value;
        this.speed = speed;
    }
}

public class CryExpressions : MonoBehaviour
{
    public SkinnedMeshRenderer m_skinnedMeshRenderer = null;
    Dictionary<string, int> blendDict = new Dictionary<string, int>();
    Mesh skinnedMesh;
    int blendShapeCount;

    List<Expression> cryBlend;
    float[] value, end;
    Dictionary<string, int> cryStepIndex;
    int[] cryStepCount;

    float frown_v, brows_v, chin_v, sneer_v;
    bool doonce, step1, step2, step3, step4, step5;

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

        cryBlend = CryExpressionInit();
        value = new float[cryBlend.Count];
        end = new float[cryBlend.Count];
        cryStepCount = new int[cryBlend.Count];
        //DebugBlendShapeIndex(cryBlend);
    }

    // Update is called once per frame
    void Update()
    {
        if (!step5)
        {
            frown_v = Mathf.Lerp(frown_v, 100.0f, 2f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(7, frown_v);

            brows_v = Mathf.Lerp(brows_v, 60.0f, 2f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(58, brows_v);
        }

        if (frown_v > 95f && !doonce)
        {
            step1 = true;
            doonce = true;
        }

        if (step1)
        {
            chin_v = Mathf.Lerp(chin_v, 100.0f, 10f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(52, chin_v);

            sneer_v = Mathf.Lerp(sneer_v, 100.0f, 10f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(12, sneer_v);

            if (chin_v > 99f)
            {
                step1 = false;
                step2 = true;
            }
        }

        if (step2)
        {
            chin_v = Mathf.Lerp(chin_v, 50.0f, 5f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(52, chin_v);

            sneer_v = Mathf.Lerp(sneer_v, 0.0f, 5f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(12, sneer_v);

            if (sneer_v < 5f)
            {
                step2 = false;
                step3 = true;
            }
        }

        if (step3)
        {
            chin_v = Mathf.Lerp(chin_v, 100.0f, 10f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(52, chin_v);

            sneer_v = Mathf.Lerp(sneer_v, 100.0f, 10f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(12, sneer_v);

            if (chin_v > 99f)
            {
                step3 = false;
                step4 = true;
            }
        }

        if (step4)
        {
            chin_v = Mathf.Lerp(chin_v, 0.0f, 5f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(52, chin_v);

            sneer_v = Mathf.Lerp(sneer_v, 0.0f, 5f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(12, sneer_v);

            if (chin_v < 5f)
            {
                step4 = false;
                step5 = true;
            }
        }

        if (step5)
        {
            frown_v = Mathf.Lerp(frown_v, 0.0f, 2f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(7, frown_v);

            brows_v = Mathf.Lerp(brows_v, 0.0f, 2f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(58, brows_v);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            step1 = false;
            step2 = false;
            step3 = false;
            step4 = false;
            step5 = false;
            doonce = false;
        }
    }

    void SetExpression(Dictionary<string, float> blendshape)
    {
        // animate character with received Blend Shape values per Blend Shape, pass on new value to character
        foreach (KeyValuePair<string, float> pair in blendshape)
        {
            //Debug.Log(pair);  // Debug.Log verrrryy slow, don't use in production (>100x slower)
            float blend_val = pair.Value;
            if (blend_val > 1.0f) blend_val = 1.0f;
            m_skinnedMeshRenderer.SetBlendShapeWeight(blendDict[pair.Key], blend_val * 100.0f);
        }

        //yield return null;
    }

    void DebugBlendShapeIndex(Dictionary<string,float[]> blendshape)
    {
        foreach (KeyValuePair<string, float[]> pair in blendshape)
        {
            Debug.Log(pair.Key+":"+blendDict[pair.Key]);  // Debug.Log verrrryy slow, don't use in production (>100x slower)
           
        }
    }

    List<Expression> CryExpressionInit()
    {
        List<Expression> cry = new List<Expression>();
        float[] frownValue = 
        {
            100f,0f
        };
        float[] BrowsValue =
        {
            100f,0f
        };
        float[] ChinLowValue =
        {
            100f,50f,100f,0f
        };
        float[] SneerValue =
        {
            100f,0f,100f,0f
        };
        float[] speed_frown =
        {
            2f
        };
        float[] speed_chin =
        {
            5f
        };
        float[] speed_sneer = {10f};
        Expression frown = new Expression(7,frownValue,speed_frown);
        Expression BrowsU_C = new Expression(58, BrowsValue,speed_frown);
        Expression ChinLower = new Expression(52, ChinLowValue, speed_chin);
        Expression Sneer = new Expression(12, SneerValue,speed_sneer);
        cry.Add(frown);
        cry.Add(BrowsU_C);
        cry.Add(ChinLower);
        cry.Add(Sneer);

        return cry;
    }

}
