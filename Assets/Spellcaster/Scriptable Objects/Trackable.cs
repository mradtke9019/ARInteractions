using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trackable", menuName = "ScriptableObjects/Trackable")]
public class Trackable : ScriptableObject
{
    [Tooltip("The hand to consider when grabbing or detecting if it is nearby.")]
    public Microsoft.MixedReality.Toolkit.Utilities.Handedness Handedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right;
    [SerializeField]
    [Tooltip("The target position of the object.")]
    public TrackedHandJoint TrackTargetPosition = TrackedHandJoint.MiddleKnuckle;
    [SerializeField]
    [Tooltip("The target orientation of the object.")]
    public TrackedHandJoint TrackTargetOrientation = TrackedHandJoint.Palm;

    [Header("Transform Offsets")]
    [Tooltip("The offset to contribute in degrees to the X Axis Orientation when updating the orientation of the object.")]
    [SerializeField]
    public float RotationOffsetXAxis = 0;
    [Tooltip("The offset to contribute in degrees to the Y Axis Orientation when updating the orientation of the object.")]
    [SerializeField]
    public float RotationOffsetYAxis = 0;
    [Tooltip("The offset to contribute in degrees to the Z Axis Orientation when updating the orientation of the object.")]
    [SerializeField]
    public float RotationOffsetZAxis = 0;
    [Tooltip("The offset to contribute to the X axis translation.")]
    [SerializeField]
    public float OffsetXAxis = 0.0f;
    [Tooltip("The offset to contribute to the Y axis translation.")]
    [SerializeField]
    public float OffsetYAxis = 0.0f;
    [Tooltip("The offset to contribute to the Z axis translation.")]
    [SerializeField]
    public float OffsetZAxis = 0.0f;
}
