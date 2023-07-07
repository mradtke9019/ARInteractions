using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class DataCollectionManager : InputActionHandler
{
    private IMixedRealityHandJointService handJointService;
    private string DataPath;
    public float RecordDuration = 1.0f;
    private float CurrentDuration = 0.0f;
    private bool Record;
    private List<HandData> rightHandData;
    private List<HandData> leftHandData;

    protected override void Update()
    {
        base.Update();

        if(Record)
        {
            HandData right = GetHandData(Handedness.Right, handJointService);
            if(right != null)
            {
                rightHandData.Add(right);
            }

            HandData left = GetHandData(Handedness.Left, handJointService);
            if (left != null)
            {
                leftHandData.Add(left);
            }
            CurrentDuration += Time.deltaTime;
            if(CurrentDuration > RecordDuration)
            {
                Debug.Log("Finished Recording Hands");
                Record = false;
                CurrentDuration = 0.0f;
                SaveHandData(rightHandData, "Right");
                SaveHandData(leftHandData, "Left");
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        Record = false;
        CurrentDuration = 0.0f;
        rightHandData = new List<HandData>();
        leftHandData = new List<HandData>();
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
    }

    public static HandData GetHandData(Handedness hand, IMixedRealityHandJointService service)
    {
        if (!service.IsHandTracked(hand))
        {
            return null;
        }
        HandData data = new HandData();
        data.Hand.Add("Label", float.MaxValue);
        var enums = Enum.GetValues(typeof(TrackedHandJoint));
        string handName = Enum.GetName(typeof(Handedness), hand);

        if(hand == Handedness.Right)
        {
            data.Hand.Add("Hand", 1.0f);
        }
        if (hand == Handedness.Left)
        {
            data.Hand.Add("Hand", -1.0f);
        }

        float index = HandPoseUtils.IndexFingerCurl(hand);
        float middle = HandPoseUtils.MiddleFingerCurl(hand);
        float ring = HandPoseUtils.RingFingerCurl(hand);
        float pinky = HandPoseUtils.PinkyFingerCurl(hand);
        float thumb = HandPoseUtils.ThumbFingerCurl(hand);

        data.Hand.Add("IndexCurl", index);
        data.Hand.Add("MiddleCurl", middle);
        data.Hand.Add("RingCurl", ring);
        data.Hand.Add("PinkyCurl", pinky);
        data.Hand.Add("ThumbCurl", thumb);

        foreach (TrackedHandJoint joint in enums)
        {
            Transform jointTransform = service.RequestJointTransform(joint, Handedness.Right);
            if(jointTransform != null)
            //if (HandJointUtils.TryGetJointPose(joint, Handedness.Right, out MixedRealityPose pose))
            {
                Transform pose = jointTransform;

                string jointName = Enum.GetName(typeof(TrackedHandJoint), joint);
                string eulerX = jointName + "XRotation";
                string eulerY = jointName + "YRotation";
                string eulerZ = jointName + "ZRotation";
                data.Hand.Add(eulerX, pose.rotation.eulerAngles.x);
                data.Hand.Add(eulerY, pose.rotation.eulerAngles.y);
                data.Hand.Add(eulerZ, pose.rotation.eulerAngles.z);

/*                string positionX = jointName + "XPosition";
                string positionY = jointName + "YPosition";
                string positionZ = jointName + "ZPosition";
                data.Hand.Add(positionX, pose.position.x);
                data.Hand.Add(positionY, pose.position.y);
                data.Hand.Add(positionZ, pose.position.z);*/



                string forwardX = jointName + "XForward";
                string forwardY = jointName + "YForward";
                string forwardZ = jointName + "ZForward";
                data.Hand.Add(forwardX, pose.forward.x);
                data.Hand.Add(forwardY, pose.forward.y);
                data.Hand.Add(forwardZ, pose.forward.z);


                string upX = jointName + "XUp";
                string upY = jointName + "YUp";
                string upZ = jointName + "ZUp";
                data.Hand.Add(upX, pose.up.x);
                data.Hand.Add(upY, pose.up.y);
                data.Hand.Add(upZ, pose.up.z);

                string rightX = jointName + "XRight";
                string rightY = jointName + "YRight";
                string rightZ = jointName + "ZRight";
                data.Hand.Add(rightX, pose.right.x);
                data.Hand.Add(rightY, pose.right.y);
                data.Hand.Add(rightZ, pose.right.z);

                string eulerLocalX = jointName + "XLocalRotation";
                string eulerLocalY = jointName + "YLocalRotation";
                string eulerLocalZ = jointName + "ZLocalRotation";
                data.Hand.Add(eulerLocalX, pose.localEulerAngles.x);
                data.Hand.Add(eulerLocalY, pose.localEulerAngles.y);
                data.Hand.Add(eulerLocalZ, pose.localEulerAngles.z);

/*                string positionLocalX = jointName + "XLocalPosition";
                string positionLocalY = jointName + "YLocalPosition";
                string positionLocalZ = jointName + "ZLocalPosition";
                data.Hand.Add(positionLocalX, pose.localPosition.x);
                data.Hand.Add(positionLocalY, pose.localPosition.y);
                data.Hand.Add(positionLocalZ, pose.localPosition.z);*/
            }

        }
        return data;
    }

    public void StartRecordingRight()
    {
        Record = true;
        CurrentDuration = 0.0f;
        rightHandData = new List<HandData>();
        leftHandData = new List<HandData>();
    }

    public void StartRecordingLeft()
    {
/*        RecordLeft = true;
        CurrentDurationLeft = 0.0f;*/
        leftHandData = new List<HandData>();
    }

    public void SaveHandData(List<HandData> data, string prefix = "")
    {
        if(data.Count <= 0 || data.All(x => x ==null))
        {
            return;
        }
        DateTime now = DateTime.Now;
        List<string> paths = new List<string>()
        {
            Application.persistentDataPath,
            Path.GetTempPath()
        };
        //DataPath = Path.Combine(DataPath);
        string fileName = prefix + now.ToString("yyyy_MM_dd.hh_mm_ss_FFF") + "HandData";
        string json = "[" + string.Join(",", data.Select(x => x.ToJsonString())) + "]";
        paths.ForEach(p =>
        {
            Debug.Log("Saving to: " + p);
            if (!Directory.Exists(p))
            {
                Directory.CreateDirectory(p);
            }
            string path = Path.Combine(p, fileName);
            File.WriteAllText(path + ".json", json);
        });
    }
}
