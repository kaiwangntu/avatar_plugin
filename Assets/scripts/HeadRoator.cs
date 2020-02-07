using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class HeadRoator : MonoBehaviour
{
    //Human muscle stuff
    HumanPoseHandler humanPoseHandler;
    HumanPose humanPose;
    Animator anim;

    // Muscle name and index lookup (See in Debug Log)
    void LookUpMuscleIndex()
    {
        string[] muscleName = HumanTrait.MuscleName;
        int i = 0;
        while (i < HumanTrait.MuscleCount)
        {
            Debug.Log(i + ": " + muscleName[i] +
                " min: " + HumanTrait.GetMuscleDefaultMin(i) + " max: " + HumanTrait.GetMuscleDefaultMax(i));
            i++;
        }
    }

    // Set character in fetus position
    void ResetMuscles()
    {
        // reset all muscles to 0
        for (int i = 0; i < humanPose.muscles.Length; i++)
        {
            //Debug.Log (humanPose.muscles [i]);
            humanPose.muscles[i] = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // https://forum.unity.com/threads/humanposehandler.430354/

        // get attached Animator controller
        anim = GetComponent<Animator>();

        // run this if you want the indexes to muscles on your character
        LookUpMuscleIndex();

        // get human pose handler
        humanPoseHandler = new HumanPoseHandler(anim.avatar, transform);

        // reference pose to pose handler
        humanPoseHandler.GetHumanPose(ref humanPose);

        // use pose information to actually set the pose;
        humanPoseHandler.SetHumanPose(ref humanPose);
    }

    void ChangeMuscleValue(int muscleIdx, float radian)
    {
        humanPose.muscles[muscleIdx] = radian;
    }

    public IEnumerator RequestHeadRotation(JObject head_pose)
    {
        //   25% neck rotation, 75% head rotation
        //Debug.Log("Head rotation: " + head_rotation[0] + ", " + head_rotation[1] + ", " + head_rotation[2]);
        // pitch (head up/down); OpenFace returns opposite values, hence *-1
        ChangeMuscleValue(9, head_pose["pose_Rx"].ToObject<float>() * -.5f);
        ChangeMuscleValue(12, head_pose["pose_Rx"].ToObject<float>() * -1);  //  * .75f

        // yaw (turn head left/right)
        ChangeMuscleValue(11, head_pose["pose_Ry"].ToObject<float>() * .5f);
        ChangeMuscleValue(14, head_pose["pose_Ry"].ToObject<float>());  //  * .75f

        // roll
        ChangeMuscleValue(10, head_pose["pose_Rz"].ToObject<float>() * -.5f);
        ChangeMuscleValue(13, head_pose["pose_Rz"].ToObject<float>() * -1);  //  * .75f

        // do the animation
        humanPoseHandler.SetHumanPose(ref humanPose);



        // Bone rotation attempt
        //Debug.Log ("" + head_rotation[0] + ", " + head_rotation[1] + ", " + head_rotation[2]);
        //Quaternion rotation = Quaternion.Euler(new Vector3(head_rotation[0], head_rotation[1], head_rotation[2]));
        //Debug.Log ("" + rotation[0] + ", " + rotation[1] + ", " + rotation[2] + ", " + rotation[3]);
        // head.Rotate (rotation);

        // https://docs.unity3d.com/ScriptReference/Quaternion-eulerAngles.html
        //rotation.eulerAngles = new Vector3 (head_rotation[0], head_rotation[1], head_rotation[2]);

        // Animator.GetBoneTransform()
        //anim.SetBoneLocalRotation(HumanBodyBones.Head, rotation);

        yield return null;
    }
}
