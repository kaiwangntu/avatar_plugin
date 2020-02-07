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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void RequestHeadRotation(Dictionary<string, float> headpose)
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
    }
}
