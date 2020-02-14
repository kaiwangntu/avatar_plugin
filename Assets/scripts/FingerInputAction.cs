using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerInputAction : MonoBehaviour
{
    public Transform camParent;
    public Transform avatar;
    public Transform avatarFlag;

    Vector2 originTapPos;
    float target;
    float angle;
    bool avatarStartRotate, camStartRotate;
    float rotSpeed = 20f;
    bool fingerMove, camRotate, expression, startWaitDoubleTap;
    bool laugh, cry;
    float angleField = 30f;
    RaycastHit hit;
    Ray ray;
    float waitDoubleTapTime;
    // Start is called before the first frame update
    void Start()
    {   
        Input.multiTouchEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                if (!startWaitDoubleTap)
                {
                    originTapPos = Input.touches[0].position;
                    //startRotate = false;
                    fingerMove = false;
                    expression = false;
                    camStartRotate = false;
                    camRotate = false;
                    ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                }
            }
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                fingerMove = true;
                if (Physics.Raycast(ray, out hit))
                {
                    //laugh or cry expression
                    if (hit.collider.gameObject.name.Equals("mouthArea1") || hit.collider.gameObject.name.Equals("mouthArea2"))
                    {
                        if (!camRotate)
                        {
                            if (Mathf.Abs(Input.touches[0].position.x - originTapPos.x) < 50f)
                            {
                                //Debug.Log("sfy111 mouth move");
                                //Debug.Log("sfy111 x:" + Mathf.Abs(Input.touches[0].position.x - originTapPos.x) + ",y:" + Mathf.Abs(Input.touches[0].position.y - originTapPos.y));
                                expression = true;
                                if (Input.touches[0].deltaPosition.y > 0)
                                {
                                    laugh = true;
                                    cry = false;
                                }
                                else
                                {
                                    cry = true;
                                    laugh = false;
                                }
                            }
                            else
                            {
                                //Debug.Log("sfy111 camera move");
                                //Debug.Log("sfy111 x:" + Mathf.Abs(Input.touches[0].position.x - originTapPos.x) + ",y:" + Mathf.Abs(Input.touches[0].position.y - originTapPos.y));
                            }
                        }
                    }
                }

                if (!expression)
                {
                    Vector3 rot = camParent.rotation.eulerAngles;
                    if((rot.y>180f&&rot.y<=360f-angleField &&Input.touches[0].deltaPosition.x<0f)||(rot.y<180f&&rot.y>angleField&&Input.touches[0].deltaPosition.x>0f))
                    {
                        camStartRotate = false;
                    }
                    else
                    {
                        camStartRotate = true;
                    }
                    if (camStartRotate)
                    {
                        angle = Input.touches[0].deltaPosition.x * 2f * Time.deltaTime;
                        //m_cam.Translate(new Vector3(Mathf.Sin(angle) * radius, 0, -(m_cam.position.z-Mathf.Cos(angle)*radius)));
                        camParent.rotation = Quaternion.Euler(0, rot.y + angle, 0);
                    }
                    camRotate = true;
                }
            }
            if (Input.touches[0].phase == TouchPhase.Ended)
            {
                //Debug.Log("unity touch end");
                
                if (fingerMove)
                {
                    if (camRotate)
                    {
                        //ShowDanMu.CamRotateExecuted = true;
                        if (camParent.eulerAngles.y > 180f) target = camParent.eulerAngles.y - 360f;
                        else target = camParent.eulerAngles.y;
                        if (Mathf.Abs(camParent.eulerAngles.y - avatar.eulerAngles.y) > 0.1f)
                        {
                            avatarStartRotate = true;
                        }
                    }
                    else if (expression)
                    {
                        if (laugh)
                        {
                            FaceExpression.laughTrigger = true;
                        }
                        else if (cry)
                        {
                            FaceExpression.cryTrigger = true;
                        }
                        FaceExpression.doFaceExpression = true;
                    }
                }
                else
                {
                    if (!startWaitDoubleTap)
                    {
                        startWaitDoubleTap = true;
                        //ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
                        if (Physics.Raycast(ray, out hit)){
                            //Debug.Log("sfyeye doface name:"+hit.collider.gameObject.name);
                            if (hit.collider.gameObject.name.Equals("noseArea"))
                            {
                                FaceExpression.funnyTrigger = true;
                            }
                            else if (hit.collider.gameObject.name.Equals("eyeAreaRight"))
                            {
                                //Debug.Log("sfyeye doface send");
                                FaceExpression.blinkEyeRight = true;
                                FaceExpression.blinkTrigger = true;
                            }
                            else if (hit.collider.gameObject.name.Equals("eyeAreaLeft"))
                            {
                                //Debug.Log("sfyeye doface send");
                                FaceExpression.blinkEyeRight = false;
                                FaceExpression.blinkTrigger = true;
                            }
                            else if (hit.collider.gameObject.name.Equals("headArea"))
                            {
                                HeadRoatorBone.nodTrigger = true;
                            }
                        }
                    }
                    else
                    {
                        if (waitDoubleTapTime < 0.3f)
                        {
                            Debug.Log("Unity double click screen");
                            waitDoubleTapTime = 0f;
                            startWaitDoubleTap = false;
                            AndroidCallUnityLib.androidActivity.Call("UnityDoubleTouchScreen");
                         }
                    }
                    
                }
                //if (Mathf.Abs(camParent.eulerAngles.y) - Mathf.Abs(avatar.eulerAngles.y) > 0.5f)
                //{
                //    startRotate = true;
                //}
            }
        }

        if (startWaitDoubleTap)
        {
            waitDoubleTapTime += Time.deltaTime;
        }
        if (waitDoubleTapTime > 0.3f)
        {
            Debug.Log("unity single tap");
            if (HeadRoatorBone.nodTrigger)
            {
                HeadRoatorBone.doBodyAction = true;
            }
            else if(FaceExpression.funnyTrigger || FaceExpression.blinkTrigger)
            {
                FaceExpression.doFaceExpression = true;
            }
            startWaitDoubleTap = false;
            waitDoubleTapTime = 0f;
        }

        if (avatarStartRotate)
        {
            if (target >= 0f)
            {
                if (avatar.eulerAngles.y >= 180f)
                {
                    avatar.rotation = Quaternion.Euler(avatar.eulerAngles.x, avatar.eulerAngles.y + rotSpeed * Time.deltaTime, avatar.eulerAngles.z);
                }
                else
                {
                    if (avatar.eulerAngles.y < target)
                    {
                        avatar.rotation = Quaternion.Euler(avatar.eulerAngles.x, avatar.eulerAngles.y + rotSpeed * Time.deltaTime, avatar.eulerAngles.z);
                        if (avatar.eulerAngles.y >= angleField)
                        {
                            avatarStartRotate = false;
                        }
                    }
                    else
                    {
                        avatar.rotation = Quaternion.Euler(avatar.eulerAngles.x, avatar.eulerAngles.y - rotSpeed * Time.deltaTime, avatar.eulerAngles.z);
                    }
                }
            }
            else
            {
                if (avatar.eulerAngles.y >= 0f && avatar.eulerAngles.y <= 180f)
                {
                    avatar.rotation = Quaternion.Euler(avatar.eulerAngles.x, avatar.eulerAngles.y - rotSpeed * Time.deltaTime, avatar.eulerAngles.z);
                }
                else
                {
                    if (avatar.eulerAngles.y - 360f < target)
                    {
                        avatar.rotation = Quaternion.Euler(avatar.eulerAngles.x, avatar.eulerAngles.y + rotSpeed * Time.deltaTime, avatar.eulerAngles.z);
                    }
                    else
                    {
                        avatar.rotation = Quaternion.Euler(avatar.eulerAngles.x, avatar.eulerAngles.y - rotSpeed * Time.deltaTime, avatar.eulerAngles.z);
                        if (avatar.eulerAngles.y <= 360f - angleField)
                        {
                            avatarStartRotate = false;
                        }
                    }
                }
            }
            if (target >= 0)
            {
                if (Mathf.Abs(avatar.eulerAngles.y - target) < rotSpeed*Time.deltaTime)
                {
                    avatarStartRotate = false;
                }
            }
            else
            {
                if (Mathf.Abs(avatar.eulerAngles.y - 360f - target) < rotSpeed*Time.deltaTime)
                {
                    avatarStartRotate = false;
                }
            }
        }
    }
}
