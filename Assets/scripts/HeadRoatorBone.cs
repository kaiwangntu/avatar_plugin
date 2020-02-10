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
    public static bool nodTrigger = false;

    bool startNod;
    float nodAngle = 0f;
    int nodCount = 0;
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
            startNod = true;
        }
        if (startNod)
        {
            HeadNod();
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
            Debug.Log("sfy111 neck rot:"+jointObj_neck.localRotation);

        }
    }

    void HeadNod()
    {
        if (nodCount < 4)
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
            jointObj_head.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    void ResetNod()
    {
        startNod = false;
        nodCount = 0;
        head_nod = true;
    }
}
