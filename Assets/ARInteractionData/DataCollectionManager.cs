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
    public string PoseName = "ThumbsUp";
    private string DataPath;
    public float RecordDuration = 1.0f;
    private float CurrentDurationRight = 0.0f;
    private float CurrentDurationLeft = 0.0f;
    private bool RecordRight;
    private bool RecordLeft;
    private List<HandData> rightHandData;
    private List<HandData> leftHandData;

    protected override void Update()
    {
        base.Update();

        if(RecordRight)
        {
            HandData hand = GetHandData(Handedness.Right);
            rightHandData.Add(hand);
            CurrentDurationRight += Time.deltaTime;
            if(CurrentDurationRight > RecordDuration)
            {
                Debug.Log("Finished Recording Right hand");
                SaveHandData(rightHandData);
                RecordRight = false;
            }
        }
        if (RecordLeft)
        {
            HandData hand = GetHandData(Handedness.Left);
            leftHandData.Add(hand);
            CurrentDurationLeft += Time.deltaTime;
            if (CurrentDurationLeft > RecordDuration)
            {
                Debug.Log("Finished Recording Left hand");
                SaveHandData(leftHandData);
                RecordLeft = false;
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        RecordRight = false;
        RecordLeft = false;
        CurrentDurationRight = 0.0f;
        CurrentDurationLeft = 0.0f;
        rightHandData = new List<HandData>();
        leftHandData = new List<HandData>();
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
    }

    public HandData GetHandData(Handedness hand)
    {
        DataPath = Application.dataPath;
        HandData data = new HandData();
        var enums = Enum.GetValues(typeof(TrackedHandJoint));
        string handName = Enum.GetName(typeof(Handedness), hand);

        DataPath = Path.Combine(DataPath, "HandData", handName);
        
        float index = HandPoseUtils.IndexFingerCurl(hand);
        float middle = HandPoseUtils.MiddleFingerCurl(hand);
        float ring = HandPoseUtils.RingFingerCurl(hand);
        float pinky = HandPoseUtils.PinkyFingerCurl(hand);
        float thumb = HandPoseUtils.ThumbFingerCurl(hand);

        data.Hand.Add(handName + "IndexCurl", index);
        data.Hand.Add(handName + "MiddleCurl", middle);
        data.Hand.Add(handName + "RingCurl", ring);
        data.Hand.Add(handName + "PinkyCurl", pinky);
        data.Hand.Add(handName + "ThumbCurl", thumb);

        foreach (TrackedHandJoint joint in enums)
        {
            Transform jointTransform = handJointService.RequestJointTransform(joint, Handedness.Right);
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

                string positionX = jointName + "XPosition";
                string positionY = jointName + "YPosition";
                string positionZ = jointName + "ZPosition";
                data.Hand.Add(positionX, pose.position.x);
                data.Hand.Add(positionY, pose.position.y);
                data.Hand.Add(positionZ, pose.position.z);



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

                string positionLocalX = jointName + "XLocalPosition";
                string positionLocalY = jointName + "YLocalPosition";
                string positionLocalZ = jointName + "ZLocalPosition";
                data.Hand.Add(positionLocalX, pose.localPosition.x);
                data.Hand.Add(positionLocalY, pose.localPosition.y);
                data.Hand.Add(positionLocalZ, pose.localPosition.z);
            }

        }
        return data;
    }

    public void StartRecordingRight()
    {
        RecordRight = true;
        CurrentDurationRight = 0.0f;
        rightHandData = new List<HandData>();
    }

    public void StartRecordingLeft()
    {
        RecordLeft = true;
        CurrentDurationLeft = 0.0f;
        leftHandData = new List<HandData>();
    }

    public void SaveHandData(List<HandData> data)
    {
        DateTime now = DateTime.Now;
        string time = now.ToString("yyyy_MM_dd.hh_mm_ss_FFF");
        string json = "[" + string.Join(",", data.Select(x => x.ToJsonString())) + "]";
        string path = Path.Combine(DataPath, time);
        if(!Directory.Exists(DataPath))
        {
            Directory.CreateDirectory(DataPath);
        }
        File.WriteAllText(path + ".json", json);
    }
}
