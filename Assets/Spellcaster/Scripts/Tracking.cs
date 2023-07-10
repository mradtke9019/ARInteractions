using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracking : MonoBehaviour
{
    [SerializeField]
    Trackable TrackingObject = null;

    [SerializeField]
    private bool HideWhenNotTracking;

    private bool _track;
    private bool _trackOrientation;
    private IMixedRealityHandJointService _handJointService;

    public void Start()
    {
        HideWhenNotTracking = true;
        _track = false;
        _trackOrientation = false;
        _handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
    }

    public void SetTracking(bool track)
    {
        _track = track;
    }
    public void SetTrackingOrientation(bool trackOrientation)
    {
        _trackOrientation = trackOrientation;
    }
    public void SetHideWhenNotTracking(bool value)
    {
        HideWhenNotTracking = value;
    }

    public void Track(Trackable trackable)
    {
        this.TrackingObject = trackable;
    }

    /// <summary>
    /// Updates the game object posotion to the specified joint on the hand.
    /// </summary>
    /// <param name="joint"></param>
    public void UpdateHandPosition(Handedness hand = Handedness.Right, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        Transform jointTransform = _handJointService.RequestJointTransform(joint, hand);
        jointTransform.Translate(new Vector3(TrackingObject.OffsetXAxis, TrackingObject.OffsetYAxis, TrackingObject.OffsetZAxis));
        this.transform.SetPositionAndRotation(jointTransform.position, this.transform.rotation);
    }

    /// <summary>
    /// Updates the game object orientation to the specified joint on the hand.
    /// </summary>
    /// <param name="joint"></param>
    public void UpdateHandOrientation(Handedness hand = Handedness.Right, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        Transform jointTransform = _handJointService.RequestJointTransform(joint, hand);

        Quaternion orientation = jointTransform.rotation;
        orientation *= Quaternion.Euler(0, TrackingObject.RotationOffsetYAxis, 0);
        orientation *= Quaternion.Euler(0, 0, TrackingObject.RotationOffsetZAxis);
        orientation *= Quaternion.Euler(TrackingObject.RotationOffsetXAxis, 0, 0);

        this.transform.SetPositionAndRotation(this.transform.position, orientation);
    }



    /// <summary>
    /// Update the position and orientation of the object if the specified hand is near the object and performing the specified gesture.
    /// </summary>
    void Update()
    {
        if (_track)
        {
            UpdateHandPosition(TrackingObject.Handedness, TrackingObject.TrackTargetPosition);
            if(_trackOrientation)
            {
                UpdateHandOrientation(TrackingObject.Handedness, TrackingObject.TrackTargetOrientation);
            }
        }
        else
        {
            if (HideWhenNotTracking)
            {
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
            }
        }
    }
}
