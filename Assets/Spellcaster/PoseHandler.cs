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

        float t = GRAB_THRESHOLD;
        bool fist = HandPoseUtils.IndexFingerCurl(hand) > t &&
            HandPoseUtils.MiddleFingerCurl(hand) > t &&
            HandPoseUtils.RingFingerCurl(hand) > t &&
            HandPoseUtils.PinkyFingerCurl(hand) > t &&
            HandPoseUtils.ThumbFingerCurl(hand) > t;
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
        }
        return Pose.None;
    }
}
