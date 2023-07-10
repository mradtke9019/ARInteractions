using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Linq;
using UnityEngine;

[Serializable]
//[CreateAssetMenu(fileName = "Gesture", menuName = "ScriptableObjects/Gesture")]
public class Gesture : MonoBehaviour
{
    //public string Label = "";
    //public GestureType _Gesture;
    public Continuity Continuity;
    public GestureRequirement Requirements;
    public GestureCallback GestureCallback;

    public Pose GetFinalPose()
    {
        return Requirements.PoseRequirements.Last().Pose;
    }
    public Handedness FinalPoseHand()
    {
        return Requirements.PoseRequirements.Last().Hand;
    }
}
