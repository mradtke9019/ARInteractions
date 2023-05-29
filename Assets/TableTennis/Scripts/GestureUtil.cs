using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

//public enum Gesture { Grab, Pinch };
public static class GestureUtil
{
    private const float GRAB_THRESHOLD = 0.3f;
    private const bool DEBUG = false;

    /// <summary>
    /// Returns whether or not the specified hand is grabbing based on a threshold.
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static bool IsGrabbing(Handedness hand, float? threshold = null)
    {
        float t;
        if(threshold == null)
        {
            t = GRAB_THRESHOLD;
        }
        else
        {
            t = threshold.Value;
        }

        bool grabbing = 
            HandPoseUtils.IndexFingerCurl(hand) > t &&
            HandPoseUtils.MiddleFingerCurl(hand) > t &&
            HandPoseUtils.RingFingerCurl(hand) > t &&
            HandPoseUtils.PinkyFingerCurl(hand) > t &&
            HandPoseUtils.ThumbFingerCurl(hand) > t;
        return grabbing;
    }

    /// <summary>
    /// Returns whether or not the specified hand is pinching based on a threshold.
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    public static bool IsPinching(Handedness hand, float? threshold = null)
    {
        float t;
        if (threshold == null)
        {
            t = GRAB_THRESHOLD;
        }
        else
        {
            t = threshold.Value;
        }

        // All fingers will be grabbing except the index finger and thumb.
        bool pinching =
            HandPoseUtils.IndexFingerCurl(hand) > t &&
            HandPoseUtils.ThumbFingerCurl(hand) > t &&
            HandPoseUtils.MiddleFingerCurl(hand) < t &&
            HandPoseUtils.RingFingerCurl(hand) < t &&
            HandPoseUtils.PinkyFingerCurl(hand) < t ; 
        return pinching;
    }

    /// <summary>
    /// Returns whether or not the given hand is performing the specified gesture.
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="gesture"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static bool IsGesturing(Handedness hand, Gesture gesture, List<object> parameters = null) 
    {
/*        switch(gesture)
        {
            case Gesture.Grab:
                if(DEBUG)
                {
                    Debug.Log("Grabbing");
                }
                return IsGrabbing(hand, parameters != null ? (float?)parameters.FirstOrDefault() : null);
            case Gesture.Pinch:
                if (DEBUG)
                {
                    Debug.Log("Pinching");
                }
                return false;
        }
*/
        return false;
    }
}
