using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class PoseHandler
{
    private const float GRAB_THRESHOLD = 0.3f;
    private const bool DEBUG = false;
    private IMixedRealityHandJointService handJointService;

    public PoseHandler()
    {
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
    }

    /// <summary>
    /// Returns what the current pose is for the given hand.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    public Pose GetPose(Handedness hand)
    {
        if (!handJointService.IsHandTracked(hand))
            return Pose.None;

        HandData handData = DataCollectionManager.GetHandData(hand, handJointService);
        return PredictPose(handData);
    }

    /// <summary>
    /// Using a pre trained machine learning model, predict from the hand data what kind of pose is being executed by the hand.
    /// </summary>
    /// <param name="handData"></param>
    /// <returns></returns>
    public Pose PredictPose(HandData handData)
    {
        Handedness hand = handData.Hand["Hand"] == 1.0f ? Handedness.Right : Handedness.Left;
        float t = GRAB_THRESHOLD;

        float index = HandPoseUtils.IndexFingerCurl(hand);
        float middle = HandPoseUtils.MiddleFingerCurl(hand);
        float ring = HandPoseUtils.RingFingerCurl(hand);
        float pinky = HandPoseUtils.PinkyFingerCurl(hand);
        float thumb = HandPoseUtils.ThumbFingerCurl(hand);


        bool fist = index > t &&
            middle > t &&
            ring > t &&
            pinky > t &&
            thumb > t;

        if (
            Similar(middle, 0.74f) &&
            Similar(index, 0.585f) &&
            Similar(ring, 0.829f) &&
            Similar(pinky, 0.84f) &&
            Similar(thumb, 0.55f))
        {
            return Pose.Fist;
        }

        if (
            Similar(middle, 0.64f) &&
            Similar(index, 0.532f) &&
            Similar(ring, 0.71f) &&
            Similar(pinky, 0.75f) &&
            Similar(thumb, 0.199f))
        {
            return Pose.ThumbsUp;
        }

        if (
            Similar(middle, 0.005f) &&
            Similar(index, 0.01f) &&
            Similar(ring, 0.01f) &&
            Similar(pinky, 0.03f) &&
            Similar(thumb, 0.35f))
        {
            return Pose.Palm;
        }
        /*
                if (fist)
                {
                    Transform handOrientation = handJointService.RequestJointTransform(TrackedHandJoint.Palm, hand);
                    float angle = 0.0f;
                    Vector3 axis = Vector3.zero;
                    handOrientation.rotation.ToAngleAxis(out angle, out axis);
                    axis.Normalize();
                    if (axis.z > 0.7f)
                    {
                        return Pose.FistPalmUp;
                    }

                    return Pose.FistPalmHorizontal;
                }*/
        return Pose.None;
    }

    private bool Similar(float value, float center, float threshold = 0.15f)
    {
        float min = center - threshold;
        float max = center + threshold;

        return value > min && value < max;
    }
}
