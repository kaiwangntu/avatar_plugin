//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;

////Press button to talk
//public class Conversation_android : MonoBehaviour
//{
//    private tts_android m_tts;
//    private AudioSource audioSource;
//    private bool shouldAvatarTalk = false;
//    private int audioClipCount = 0;
//    public Button recordButton;
//    public Dropdown change_mode;
//    public RectTransform inputField;
//    private action_android action;
//    bool isTalk = true;
//    bool speechDetect, buttonRelease;

//    bool faceExpression = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        //get tts
//        m_tts = GameObject.Find("tts").GetComponent<tts_android>();
//        action = GameObject.Find("avatar").GetComponent<action_android>();
//        //get AudioSource
//        audioSource = GameObject.Find("avatar").GetComponent<AudioSource>();
//        audioSource.loop = false;

//        if (change_mode.value == 0)
//        {
//            inputField.gameObject.SetActive(false);
//            isTalk = true;
//            Debug.Log("sfy unity chat");
//        }

//        change_mode.interactable = true;
//        change_mode.onValueChanged.AddListener((int v) =>
//        {
//            switch (v)
//            {
//                case 0://talk
//                    inputField.gameObject.SetActive(false);
//                    recordButton.transform.GetChild(0).GetComponent<Text>().text = "按住说话";
//                    isTalk = true;
//                    Debug.Log("sfy unity chat");
//                    break;
//                case 1: //read content in the text box
//                    inputField.gameObject.SetActive(true);
//                    recordButton.transform.GetChild(0).GetComponent<Text>().text = "播报";
//                    isTalk = false;
//                    Debug.Log("sfy unity read");
//                    break;
//            }

//        });

//        #region
//        //button longer press to talk
//        EventTrigger trigger = recordButton.gameObject.AddComponent<EventTrigger>();
//        //Press button to record
//        var pointerDown = new EventTrigger.Entry();
//        pointerDown.eventID = EventTriggerType.PointerDown;
//        pointerDown.callback.AddListener((e) =>
//        {
//            if (isTalk)
//            {
//                Debug.Log("SFY button pressed");
//                shouldAvatarTalk = false;
//                ResetAudioSource();
//                AndroidActivity.androidActivity.Call("RecordStart", true);
//            }
//        });
//        //Release button to stop record
//        var pointerUp = new EventTrigger.Entry();
//        pointerUp.eventID = EventTriggerType.PointerUp;
//        pointerUp.callback.AddListener((e) =>
//        {
//            Debug.Log("SFY button released");
//            buttonRelease = true;
//            Debug.Log("sfy faceExpression:"+faceExpression+",shouldAvatarTalk:"+shouldAvatarTalk);
//        });
//        trigger.triggers.Add(pointerDown);
//        trigger.triggers.Add(pointerUp);
//        #endregion
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(speechDetect && buttonRelease)
//        {
//            if (faceExpression)
//            {
//                FaceExpression.doFaceExpression = true;
//            }
//            else
//            {
//                if (isTalk)
//                {
//                    AndroidActivity.androidActivity.Call("RecordEnd", true);
//                    shouldAvatarTalk = true;
//                }
//                else
//                {
//                    ResetAudioSource();
//                    AndroidActivity.androidActivity.Call("TextToRead", inputField.GetComponent<InputField>().text);
//                    shouldAvatarTalk = true;
//                }
//            }
//            speechDetect = false;
//            buttonRelease = false;
//        }
//        //Debug.Log("sfy isAudioSource Playing:"+audioSource.isPlaying);
//        //Play tts result

//        if (shouldAvatarTalk)
//        {
//            if (m_tts.GetAudioClipNum() > 0 && audioClipCount < m_tts.GetAudioClipNum())
//            {
//                if (!audioSource.isPlaying)
//                {
//                    audioSource.clip = m_tts.GetAudioClip(audioClipCount);
//                    audioSource.Play();
//                    audioClipCount++;
//                    //Debug.Log("sfy set audioclip");
//                }
//            }
//        }

//        action.isSpeaking = audioSource.isPlaying;

//    }

//    public void PlayFaceExpression(string flag)
//    {
//        if (flag.Equals("true"))
//            faceExpression = true;
//        else faceExpression = false;
//        speechDetect = true;
//    }

//    void ResetAudioSource(){
//        m_tts.ClearAudioList();
//        audioSource.Stop();
//        audioSource.clip = null;
//        audioClipCount = 0;
//        action.isSpeaking = false;
//    }

//}
