using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//[CreateAssetMenu(fileName = "GestureRequirement", menuName = "ScriptableObjects/PoseRequirement")]
public class PoseRequirement //: ScriptableObject
{
    [SerializeField]
    public Pose Pose;
    [SerializeField]
    public float Duration = 0.0f;
    [SerializeField]
    public Handedness Hand = Handedness.Any;
}
