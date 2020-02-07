using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FaceExpression: MonoBehaviour
{
    public SkinnedMeshRenderer m_skinnedMeshRenderer = null;

    public static bool doFaceExpression;

    AudioSource audioSource;
    private AudioClip cryClip, laughClip;
    private string cry_url = Application.streamingAssetsPath + "/cry.wav";
    private string laugh_url = Application.streamingAssetsPath + "/laugh.wav";

    public static bool cryTrigger, laughTrigger, funnyTrigger, blinkTrigger, blinkEyeRight;

    bool startCry, startLaugh, startFunny, startBlink;

    int blendshapeCount;

    //cry variable
    bool doonce;
    bool[] cryStep = new bool[5];
    float frown_cry, brow_cry, chin_cry, sneer_cry, cheek_cry, eye_cry;

    //laugh variable
    float smile_laugh, cheek_laugh, chin_laugh, eye_laugh, brow_laugh;
    bool smile;

    //funny face variable
    float browU_funny, browD_funny, mouth_funny, cheek_funny;
    bool brow_down;

    //blink variable
    float eye_blink;
    bool eye_open = true;
    int blinkCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        blendshapeCount = m_skinnedMeshRenderer.sharedMesh.blendShapeCount;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        StartCoroutine(loadWav());//load cry and laugh wav file

    }

    // Update is called once per frame
    void Update()
    {
        if (doFaceExpression && (cryTrigger||Input.GetKeyDown(KeyCode.Alpha1)))
        {
            //cry
            ShowDanMu.CryExecuted = true;
            cryTrigger = false;
            ResetExpression();
            ResetCry();
            startCry = true;
            audioSource.clip = cryClip;
            audioSource.Play();
        }
        else if(doFaceExpression &&(laughTrigger || Input.GetKeyDown(KeyCode.Alpha2)))
        {
            //laugh
            ShowDanMu.LaughExecuted = true;
            laughTrigger = false;
            ResetExpression();
            ResetLaugh();
            startLaugh = true;
            audioSource.clip = laughClip;
            audioSource.Play();
        }
        else if(doFaceExpression &&(funnyTrigger || Input.GetKeyDown(KeyCode.Alpha3)))
        {
            //funny face
            ShowDanMu.FunnyExecuted = true;
            funnyTrigger = false;
            ResetExpression();
            ResetFunny();
            audioSource.clip = null;
            startFunny = true;
        }
        else if(doFaceExpression && blinkTrigger)
        {
            ShowDanMu.BlinkExecuted = true;
            blinkTrigger = false;
            ResetExpression();
            ResetBlink();
            audioSource.clip = null;
            startBlink = true;
        }

        if (startCry)
        {
            Cry();
        }
        else if (startLaugh)
        {
            Laugh();
        }
        else if (startFunny)
        {
            FunnyFace();
        }
        else if (startBlink)
        {
            EyeBlink(blinkEyeRight);
        }
    }

    void Cry()
    {
        if (!cryStep[4])
        {
            frown_cry = Mathf.Lerp(frown_cry, 100.0f, 5f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(7, frown_cry);

            brow_cry = Mathf.Lerp(brow_cry, 60.0f, 5f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(58, brow_cry);
        }

        if (frown_cry > 95f && !doonce)
        {
            cryStep[0] = true;
            doonce = true;
        }

        if (cryStep[0])
        {
            chin_cry = Mathf.Lerp(chin_cry, 100.0f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(52, chin_cry);

            sneer_cry = Mathf.Lerp(sneer_cry, 100.0f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(12, sneer_cry);

            cheek_cry = Mathf.Lerp(cheek_cry, 50f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(54, cheek_cry);
            m_skinnedMeshRenderer.SetBlendShapeWeight(53, cheek_cry);

            eye_cry = Mathf.Lerp(eye_cry, 50f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(44, eye_cry);
            m_skinnedMeshRenderer.SetBlendShapeWeight(45, eye_cry);

            if (chin_cry > 99f)
            {
                cryStep[0] = false;
                cryStep[1] = true;
            }
        }

        if (cryStep[1])
        {
            chin_cry = Mathf.Lerp(chin_cry, 50.0f, 6f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(52, chin_cry);

            sneer_cry = Mathf.Lerp(sneer_cry, 0.0f, 6f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(12, sneer_cry);

            cheek_cry = Mathf.Lerp(cheek_cry, 0f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(54, cheek_cry);
            m_skinnedMeshRenderer.SetBlendShapeWeight(53, cheek_cry);

            eye_cry = Mathf.Lerp(eye_cry, 0f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(44, eye_cry);
            m_skinnedMeshRenderer.SetBlendShapeWeight(45, eye_cry);

            if (sneer_cry < 5f)
            {
                cryStep[1] = false;
                cryStep[2] = true;
            }
        }

        if (cryStep[2])
        {
            chin_cry = Mathf.Lerp(chin_cry, 100.0f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(52, chin_cry);

            sneer_cry = Mathf.Lerp(sneer_cry, 100.0f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(12, sneer_cry);

            cheek_cry = Mathf.Lerp(cheek_cry, 50f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(54, cheek_cry);
            m_skinnedMeshRenderer.SetBlendShapeWeight(53, cheek_cry);

            eye_cry = Mathf.Lerp(eye_cry, 50f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(44, eye_cry);
            m_skinnedMeshRenderer.SetBlendShapeWeight(45, eye_cry);

            if (chin_cry > 99f)
            {
                cryStep[2] = false;
                cryStep[3] = true;
            }
        }

        if (cryStep[3])
        {
            chin_cry = Mathf.Lerp(chin_cry, 0.0f, 10f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(52, chin_cry);

            sneer_cry = Mathf.Lerp(sneer_cry, 0.0f, 10f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(12, sneer_cry);

            cheek_cry = Mathf.Lerp(cheek_cry, 0f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(54, cheek_cry);
            m_skinnedMeshRenderer.SetBlendShapeWeight(53, cheek_cry);

            eye_cry = Mathf.Lerp(eye_cry, 0f, 15f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(44, eye_cry);
            m_skinnedMeshRenderer.SetBlendShapeWeight(45, eye_cry);

            if (chin_cry < 5f)
            {
                cryStep[3] = false;
                cryStep[4] = true;
            }
        }

        if (cryStep[4])
        {
            frown_cry = Mathf.Lerp(frown_cry, 0.0f, 10f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(7, frown_cry);

            brow_cry = Mathf.Lerp(brow_cry, 0.0f, 10f * Time.deltaTime);
            m_skinnedMeshRenderer.SetBlendShapeWeight(58, brow_cry);

            if (frown_cry - 0f < 1f)
            {
                doFaceExpression = false;
                ResetCry();
                ResetExpression();
            }
        }
    }


    void Laugh()
    {
        if (40.0f - smile_laugh > 0.5f || 70f - chin_laugh > 0.5f || 50f - cheek_laugh > 0.5f || 100f - eye_laugh > 0.5f || 20f - brow_laugh > 0.5f)
        {

        }
        else
        {
            smile = false;
        }
        if (smile)
        {
            smile_laugh = Mathf.Lerp(smile_laugh, 40.0f, 5f * Time.deltaTime);
            chin_laugh = Mathf.Lerp(chin_laugh, 70f, 5f * Time.deltaTime);
            cheek_laugh = Mathf.Lerp(cheek_laugh, 50f, 5f * Time.deltaTime);
            eye_laugh = Mathf.Lerp(eye_laugh, 100f, 5f*Time.deltaTime);
            brow_laugh = Mathf.Lerp(brow_laugh, 20f, 5f * Time.deltaTime);
        }
        else
        {
            smile_laugh = Mathf.Lerp(smile_laugh, 0.0f, 5f * Time.deltaTime);
            chin_laugh = Mathf.Lerp(chin_laugh, 0f, 5f * Time.deltaTime);
            cheek_laugh = Mathf.Lerp(cheek_laugh, 0f, 5f * Time.deltaTime);
            eye_laugh = Mathf.Lerp(eye_laugh, 0f, 5f*Time.deltaTime);
            brow_laugh = Mathf.Lerp(brow_laugh, 0f, 5f * Time.deltaTime);
            if (smile_laugh - 0f < 1f && chin_laugh - 0f < 1f && cheek_laugh - 0f < 1f && eye_laugh - 0f < 1f && brow_laugh - 0f < 1f)
            {
                doFaceExpression = false;
                ResetLaugh();
                ResetExpression();
            }
        }

        m_skinnedMeshRenderer.SetBlendShapeWeight(16, smile_laugh);
        m_skinnedMeshRenderer.SetBlendShapeWeight(17, smile_laugh);
        m_skinnedMeshRenderer.SetBlendShapeWeight(51, chin_laugh);
        m_skinnedMeshRenderer.SetBlendShapeWeight(54, cheek_laugh);
        m_skinnedMeshRenderer.SetBlendShapeWeight(53, cheek_laugh);
        m_skinnedMeshRenderer.SetBlendShapeWeight(44, eye_laugh);
        m_skinnedMeshRenderer.SetBlendShapeWeight(45, eye_laugh);
        m_skinnedMeshRenderer.SetBlendShapeWeight(59, brow_laugh);
        m_skinnedMeshRenderer.SetBlendShapeWeight(60, brow_laugh);
    }

    void FunnyFace()
    {
        if (brow_down)
        {
            browU_funny = Mathf.Lerp(browU_funny, 0.0f, 10f * Time.deltaTime);
            browD_funny = Mathf.Lerp(browD_funny, 0f, 10f * Time.deltaTime);
            mouth_funny = Mathf.Lerp(mouth_funny, 0f, 10f * Time.deltaTime);
            cheek_funny = Mathf.Lerp(cheek_funny, 0f, 10f * Time.deltaTime);
        }
        else
        {
            browU_funny = Mathf.Lerp(browU_funny, 100.0f, 10f * Time.deltaTime);
            browD_funny = Mathf.Lerp(browD_funny, 60f, 10f * Time.deltaTime);
            mouth_funny = Mathf.Lerp(mouth_funny, 50f, 10f * Time.deltaTime);
            cheek_funny = Mathf.Lerp(cheek_funny, 50f, 10f * Time.deltaTime);
        }
        m_skinnedMeshRenderer.SetBlendShapeWeight(57, browU_funny);
        m_skinnedMeshRenderer.SetBlendShapeWeight(59, browD_funny);
        m_skinnedMeshRenderer.SetBlendShapeWeight(53, cheek_funny);
        m_skinnedMeshRenderer.SetBlendShapeWeight(18, mouth_funny);
        if (100f - browU_funny < 0.5f)
        {
            brow_down = true;
        }
        if (browU_funny - 0f < 0.5f)
        {
            brow_down = false;
            doFaceExpression = false;
            //startFunny = false;
            ResetExpression();
        }
    }

    void EyeBlink(bool isRightEye)
    {
        //Debug.Log("sfyeye blinkCount:"+blinkCount);
        if (blinkCount < 3)
        {
            if (eye_open)
            {
                eye_blink = Mathf.Lerp(eye_blink, 100f, 30f * Time.deltaTime);
            }
            else
            {
                eye_blink = Mathf.Lerp(eye_blink, 0f, 30f * Time.deltaTime);
            }
            if (isRightEye)
            {
                m_skinnedMeshRenderer.SetBlendShapeWeight(48, eye_blink);//R
            }
            else
            {
                m_skinnedMeshRenderer.SetBlendShapeWeight(49, eye_blink);//L
            }
            if (100f - eye_blink < 1f)
            {
                eye_open = false;
            }
            if (eye_blink < 1f)
            {
                eye_open = true;
                blinkCount++;
            }
        }
        else
        {
            doFaceExpression = false;
            ResetBlink();
            ResetExpression();
        }
    }

    void ResetCry()
    {
        for (int i = 0; i < cryStep.Length; i++)
            cryStep[i] = false;
        doonce = false;
        frown_cry = 0f;
        brow_cry = 0f;
        chin_cry = 0f;
        sneer_cry = 0f;
    }

    void ResetLaugh()
    {
        smile = true;
        smile_laugh = 0f;
        chin_laugh = 0f;
        cheek_laugh = 0f;
        eye_laugh = 0f;
        brow_laugh = 0f;
    }

    void ResetFunny()
    {
        brow_down = false;
        browU_funny = 0f;
        browD_funny = 0f;
        mouth_funny = 0f;
        cheek_funny = 0f;
    }

    void ResetBlink()
    {
        eye_open = true;
        eye_blink = 0f;
        blinkCount = 0;
    }

    void ResetExpression()
    {
        startCry = false;
        startLaugh = false;
        startFunny = false;
        startBlink = false;
        for(int i = 0; i < blendshapeCount; i++)
        {
            m_skinnedMeshRenderer.SetBlendShapeWeight(i, 0f);
        }
    }

    IEnumerator loadWav()
    {
        Debug.Log("loadWav");
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(laugh_url, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log("load error" + www.error);
            }
            else
            {
                Debug.Log("load laugh wav");
                laughClip = DownloadHandlerAudioClip.GetContent(www);
            }

        }
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(cry_url, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log("load error" + www.error);
            }
            else
            {
                Debug.Log("load cry wav");
                cryClip = DownloadHandlerAudioClip.GetContent(www);
            }

        }
    }
}
