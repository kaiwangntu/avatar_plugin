using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadRoatorBone : MonoBehaviour
{
    //Transform head;
    public Transform jointObj_head;
    public Transform jointObj_neck;
    public float headRotCorrection;
    public float neckRotCorrection;

    public static bool doBodyAction = false;
    public static bool nodTrigger, nodSyncTrigger, shakeSyncTrigger;

    bool startNod, startNodSync, startShake, startShakeSync;
    float shakeTarget = 0f;
    float nodAngle = 0f, shakeAngle = 0f;
    int nodCount = 0, shakeCount = 0;
    bool head_nod = true;

    // Start is called before the first frame update
    void Start()
    {
        jointObj_neck.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(doBodyAction && nodTrigger)
        {
            nodTrigger = false;
            ResetNod();
            startNod = true;
        }else if(doBodyAction && nodSyncTrigger)
        {
            nodSyncTrigger = false;
            ResetNod();
            startNodSync = true;
        }
        else if(doBodyAction && shakeSyncTrigger)
        {
            shakeSyncTrigger = false;
            ResetShake();
            shakeTarget = -7f;
            startShakeSync = true;
        }

        if (startNod)
        {
            HeadNod(4);
        }
        else if (startNodSync)
        {
            HeadNod(1);
        }
        else if (startShakeSync)
        {
            HeadShake(2);
        }
    }

    public void RequestHeadRotation(Dictionary<string, float> headpose)
    {
        if (!doBodyAction)
        {
            // New attempt bone rotation
            float multiplier_head = 0.65f;
            float multiplier_neck = 0.35f;
            //Vector3 rot = new Vector3(headpose["pose_Rx"] * Mathf.Rad2Deg, headpose["pose_Ry"] * Mathf.Rad2Deg, headpose["pose_Rz"] * Mathf.Rad2Deg);
            Vector3 rot = new Vector3(headpose["pose_Ry"] * Mathf.Rad2Deg, headpose["pose_Rz"] * Mathf.Rad2Deg, headpose["pose_Rx"] * Mathf.Rad2Deg);

            // -23.86f
            jointObj_head.localRotation = Quaternion.Euler((rot * multiplier_head) + new Vector3(headRotCorrection, 0, 0));
            // +32.928f
            jointObj_neck.localRotation = Quaternion.Euler((rot * multiplier_neck) + new Vector3(neckRotCorrection, 0, 0));
            //Debug.Log("sfy111 neck rot:"+jointObj_neck.localRotation);

        }
    }

    void HeadNod(int nodTimes)
    {
        if (nodCount < nodTimes)
        {
            if (head_nod)
            {
                nodAngle = Mathf.Lerp(nodAngle, -20f, 10f * Time.deltaTime);
            }
            else
            {
                nodAngle = Mathf.Lerp(nodAngle, 2f, 10f * Time.deltaTime);
            }

            jointObj_head.localRotation = Quaternion.Euler(0f, 0f, nodAngle);
           
            if (nodAngle < -19f)
            {
                head_nod = false;
            }
            if (nodAngle > 0f)
            {
                head_nod = true;
                nodCount++;
            }
        }
        else
        {
            doBodyAction = false;
            ResetNod();
        }
    }

    void HeadShake(int shakeTimes)
    {
        if (shakeCount < shakeTimes)
        {
            shakeAngle = Mathf.Lerp(shakeAngle, shakeTarget, 10f * Time.deltaTime);

            jointObj_head.localRotation = Quaternion.Euler(shakeAngle, 0f, 0f);

            if (shakeAngle < -6f)
            {
                shakeTarget = 7f;
            }
            if (shakeAngle > 6f)
            {
                if (shakeCount == shakeTimes - 1)
                {
                    shakeTarget = 0f;
                }
                else
                {
                    shakeTarget = -7f;
                    shakeCount++;
                }
            }
            if(shakeCount == shakeTimes-1 && shakeAngle < 1f && shakeTarget==0f)
            {
                shakeCount++;
            }
        }
        else
        {
            doBodyAction = false;
            ResetShake();
        }
    }

    void ResetNod()
    {
        startNod = false;
        startNodSync = false;
        nodCount = 0;
        head_nod = true;
        nodAngle = 0f;
        jointObj_head.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void ResetShake()
    {
        startShake = false;
        startShakeSync = false;
        shakeCount = 0;
        shakeAngle = 0f;
        jointObj_head.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
