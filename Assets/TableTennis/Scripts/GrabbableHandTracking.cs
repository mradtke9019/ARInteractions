using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabbableHandTracking : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [Tooltip("The gesture needed to interact with this object.")]
    private Gesture GestureRequired = Gesture.None;
    [SerializeField]
    [Tooltip("The threshold for considering if something is gripped.")]
    private float GrabGripThreshold = 0.3f;
    [SerializeField]
    [Tooltip("The threshold distance for interactions to be considered.")]
    private float GrabDistanceThreshold = 0.2f;
    [SerializeField]
    [Tooltip("The hand to consider when grabbing or detecting if it is nearby.")]
    private Microsoft.MixedReality.Toolkit.Utilities.Handedness Handedness = Microsoft.MixedReality.Toolkit.Utilities.Handedness.Right;
    [SerializeField]
    [Tooltip("The target position of the object.")]
    private TrackedHandJoint TrackTargetPosition = TrackedHandJoint.MiddleKnuckle;
    [SerializeField]
    [Tooltip("The target orientation of the object.")]
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


    private IMixedRealityHandJointService handJointService;
    private IMixedRealityHandJointService HandJointService =>
        handJointService ??
        (handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());


    private Rigidbody _rigidbody;
    private void Start()
    {
        Vector3 offsetAbsolute = new Vector3(Mathf.Abs(OffsetXAxis), Mathf.Abs(OffsetYAxis), Mathf.Abs(OffsetZAxis));
        float distance = offsetAbsolute.magnitude;

        if(distance > GrabDistanceThreshold)
        {
            Debug.LogError($"Offset on {this.gameObject.name} will always put it out of grab distance. Adjust offset or grab distance.");
        }

        _rigidbody = this.GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            _rigidbody = this.AddComponent<Rigidbody>();
        }
        _rigidbody.isKinematic = true;
        _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rigidbody.useGravity = false;
    }

    /// <summary>
    /// Updates the game object posotion to the specified joint on the hand.
    /// </summary>
    /// <param name="joint"></param>
    public void UpdateHandPosition(Handedness hand = Handedness.Right, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        Transform jointTransform = HandJointService.RequestJointTransform(joint, hand);
        jointTransform.Translate(new Vector3(OffsetXAxis, OffsetYAxis, OffsetZAxis));
        //Vector3 offset =  new Vector3(OffsetXAxis, OffsetYAxis, OffsetZAxis);
        //this.transform.SetPositionAndRotation(jointTransform.position, this.transform.rotation);
        _rigidbody.MovePosition(jointTransform.position);
    }

    /// <summary>
    /// Updates the game object orientation to the specified joint on the hand.
    /// </summary>
    /// <param name="joint"></param>
    public void UpdateHandOrientation(Handedness hand = Handedness.Right, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        Transform jointTransform = HandJointService.RequestJointTransform(joint, hand);

        Quaternion orientation = jointTransform.rotation;
        orientation *= Quaternion.Euler(0, RotationOffsetYAxis, 0);
        orientation *= Quaternion.Euler(0, 0, RotationOffsetZAxis);
        orientation *= Quaternion.Euler(RotationOffsetXAxis, 0, 0);

        _rigidbody.MoveRotation(orientation);
    }

    /// <summary>
    /// Returns whether or not the given hand's joint is close to the object based on a distance. https://localjoost.github.io/Basic-hand-gesture-recognition-with-MRTK-on-HoloLens-2/
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="distance"></param>
    /// <param name="joint"></param>
    /// <returns></returns>
    public bool IsGrabbing(Handedness hand, Gesture gesture, float distance, float grabThreshold, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        return IsNearby(hand, distance, joint) && GestureUtil.IsGesturing(hand, gesture, new List<object>() { grabThreshold });
    }

    public bool IsNearby(Handedness hand, float distance, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        if(!HandJointService.IsHandTracked(hand))
        {
            return false;
        }

        Transform jointTransform = HandJointService.RequestJointTransform(TrackedHandJoint.Palm, hand);

        float d = Vector3.Distance(jointTransform.position, this.gameObject.transform.position);
        if(d < distance)
        {
            //Debug.Log("Hand near: " + d);
        }
        return d < distance;
    }

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
        if(IsGrabbing(Handedness, GestureRequired, GrabDistanceThreshold, GrabGripThreshold, TrackTargetPosition))
        {
            UpdateHandPosition(Handedness, TrackTargetPosition);
            UpdateHandOrientation(Handedness, TrackTargetOrientation);
        }
    }
}
