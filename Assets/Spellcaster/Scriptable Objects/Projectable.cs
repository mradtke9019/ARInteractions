using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

[CreateAssetMenu(fileName = "Projectable", menuName = "ScriptableObjects/Projectable")]
public class Projectable : ScriptableObject
{
    public Handedness Hand;
    public TrackedHandJoint Origin;
    public TrackedHandJoint Direction;
    public float DirectionOffsetX = 0;
    public float DirectionOffsetY = 0;
    public float DirectionOffsetZ = 0;
    public float Speed;
    public float Duration;
    public Vector3 GetOrigin()
    {
        var _handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        Transform jointTransform = _handJointService.RequestJointTransform(Origin, Hand);
        return jointTransform.position;
    }

    public Vector3 GetDirection()
    {
        var _handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        Transform jointTransform = _handJointService.RequestJointTransform(Direction, Hand);
        Vector3 result = -jointTransform.up;

        return result;
        /*
        orientation *= Quaternion.Euler(0, DirectionOffsetY, 0);
        orientation *= Quaternion.Euler(0, 0, DirectionOffsetZ);
        orientation *= Quaternion.Euler(DirectionOffsetX, 0, 0);*/

        //return orientation.eulerAngles.normalized;
    }
}
