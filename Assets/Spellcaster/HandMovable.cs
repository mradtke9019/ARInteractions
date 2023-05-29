using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class HandMovable : MonoBehaviour
{
    private IMixedRealityHandJointService _handJointService;
    private Handedness _trackedHand;
    public void Start()
    {
        _handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        _trackedHand = Handedness.Right;
    }
    public void Test()
    {

    }
    public void TrackHand(Handedness hand)
    {
        _trackedHand = hand;
    }
    public void MoveToTrackedHand(TrackedHandJoint joint)
    {
        var h = HandJointUtils.FindHand(_trackedHand);
        Transform jointTransform = _handJointService.RequestJointTransform(joint, _trackedHand);
        this.transform.SetPositionAndRotation(jointTransform.position, this.transform.rotation);
    }
}
