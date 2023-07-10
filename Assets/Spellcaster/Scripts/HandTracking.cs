using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    [Tooltip("The hand to consider when grabbing or detecting if it is nearby.")]
    private Microsoft.MixedReality.Toolkit.Utilities.Handedness Handedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right;
    [SerializeField]
    [Tooltip("The target position of the object.")]
    private TrackedHandJoint TrackTargetPosition = TrackedHandJoint.MiddleKnuckle;

    [SerializeField]
    [Tooltip("The target orientation of the object.")]
#pragma warning disable CS0414 // The field 'HandTracking.TrackTargetOrientation' is assigned but its value is never used
    private TrackedHandJoint TrackTargetOrientation = TrackedHandJoint.Palm;

    [Header("Transform Offsets")]
    [Tooltip("The offset to contribute in degrees to the X Axis Orientation when updating the orientation of the object.")]
    [SerializeField]
    private float RotationOffsetXAxis = 180.0f;
    [Tooltip("The offset to contribute in degrees to the Y Axis Orientation when updating the orientation of the object.")]
    [SerializeField]
    private float RotationOffsetYAxis = -45.0f;
    [Tooltip("The offset to contribute in degrees to the Z Axis Orientation when updating the orientation of the object.")]
    [SerializeField]
    private float RotationOffsetZAxis = -45.0f;
    [Tooltip("The offset to contribute to the X axis translation.")]
    [SerializeField]
    private float OffsetXAxis = 0.0f;
    [Tooltip("The offset to contribute to the Y axis translation.")]
    [SerializeField]
    private float OffsetYAxis = 0.0f;
    [Tooltip("The offset to contribute to the Z axis translation.")]
    [SerializeField]
    private float OffsetZAxis = 0.0f;

    [SerializeField]
    private bool HideWhenNotTracking = true;

    private bool _track;
    private IMixedRealityHandJointService _handJointService;

    public void Start()
    {
        _track = false;
        _handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
    }

    public void SetTracking(bool track) 
    {
        _track = track;
    }
    public void SetHideWhenNotTracking(bool value)
    {
        HideWhenNotTracking = value;
    }


    public void TrackHand(Handedness hand)
    {
        this.Handedness = hand;
    }
    public void TrackJoint(TrackedHandJoint joint)
    {
        this.TrackTargetPosition = joint;
    }

    /// <summary>
    /// Updates the game object posotion to the specified joint on the hand.
    /// </summary>
    /// <param name="joint"></param>
    public void UpdateHandPosition(Handedness hand = Handedness.Right, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        Transform jointTransform = _handJointService.RequestJointTransform(joint, hand);
        jointTransform.Translate(new Vector3(OffsetXAxis, OffsetYAxis, OffsetZAxis));
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
        orientation *= Quaternion.Euler(0, RotationOffsetYAxis, 0);
        orientation *= Quaternion.Euler(0, 0, RotationOffsetZAxis);
        orientation *= Quaternion.Euler(RotationOffsetXAxis, 0, 0);

        this.transform.SetPositionAndRotation(this.transform.position, orientation);
        //_rigidbody.MoveRotation(orientation);
    }

    #region Offsets
    public void SetRotationOffsetXAxis(float value)
    {
        RotationOffsetXAxis = value;
    }
    public void SetRotationOffsetYAxis(float value)
    {
        RotationOffsetYAxis = value;
    }
    public void SetRotationOffsetZAxis(float value)
    {
        RotationOffsetZAxis = value;
    }
    public void SetOffsetXAxis(float value)
    {
        OffsetXAxis = value;
    }
    public void SetOffsetYAxis(float value)
    {
        OffsetYAxis = value;
    }
    public void SetOffsetZAxis(float value)
    {
        OffsetZAxis = value;
    }
    #endregion

    #region Incremental Functions
    public void IncrementRotationOffsetXAxis(float value)
    {
        RotationOffsetXAxis += value;
    }
    public void IncrementRotationOffsetYAxis(float value)
    {
        RotationOffsetYAxis += value;
    }
    public void IncrementRotationOffsetZAxis(float value)
    {
        RotationOffsetZAxis += value;
    }
    public void IncrementOffsetXAxis(float value)
    {
        OffsetXAxis += value;
    }
    public void IncrementOffsetYAxis(float value)
    {
        OffsetYAxis += value;
    }
    public void IncrementOffsetZAxis(float value)
    {
        OffsetZAxis += value;
    }
    #endregion

    #region Decremental Regions
    public void DecrementRotationOffsetXAxis(float value)
    {
        RotationOffsetXAxis -= value;
    }
    public void DecrementRotationOffsetYAxis(float value)
    {
        RotationOffsetYAxis -= value;
    }
    public void DecrementRotationOffsetZAxis(float value)
    {
        RotationOffsetZAxis -= value;
    }
    public void DecrementOffsetXAxis(float value)
    {
        OffsetXAxis -= value;
    }
    public void DecrementOffsetYAxis(float value)
    {
        OffsetYAxis -= value;
    }
    public void DecrementOffsetZAxis(float value)
    {
        OffsetZAxis -= value;
    }
    #endregion


    /// <summary>
    /// Update the position and orientation of the object if the specified hand is near the object and performing the specified gesture.
    /// </summary>
    void Update()
    {
        if(_track)
        {
            UpdateHandPosition(Handedness, TrackTargetPosition);
            //UpdateHandOrientation(Handedness, TrackTargetOrientation);
        }
        else
        {
            if(HideWhenNotTracking)
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
