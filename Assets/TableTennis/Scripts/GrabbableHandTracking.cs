using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using UnityEngine.XR;

public class GrabbableHandTracking : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private float GrabThreshold = 0.4f;
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

    [SerializeField]
    private const float GrabDistanceThreshold = 0.3f;

    private IMixedRealityHandJointService handJointService;
    private IMixedRealityHandJointService HandJointService =>
        handJointService ??
        (handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>());

    /// <summary>
    /// Updates the game object posotion to the specified joint on the hand.
    /// </summary>
    /// <param name="joint"></param>
    public void UpdateHandPosition(Handedness hand = Handedness.Right, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        Transform jointTransform = HandJointService.RequestJointTransform(joint, hand);
        jointTransform.Translate(new Vector3(OffsetXAxis, OffsetYAxis, OffsetZAxis));
        //Vector3 offset =  new Vector3(OffsetXAxis, OffsetYAxis, OffsetZAxis);
        this.transform.SetPositionAndRotation(jointTransform.position, this.transform.rotation);
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

        this.transform.SetPositionAndRotation(this.transform.position, orientation);
    }
    void Start()
    {
        
    }

    /// <summary>
    /// Returns whether or not the given hand's joint is close to the object based on a distance. https://localjoost.github.io/Basic-hand-gesture-recognition-with-MRTK-on-HoloLens-2/
    /// </summary>
    /// <param name="hand"></param>
    /// <param name="distance"></param>
    /// <param name="joint"></param>
    /// <returns></returns>
    public bool IsGrabbing(Handedness hand, float distance, float grabThreshold, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        if(!IsNearby(hand, distance, joint))
        {
            return false;
        }
        bool grabbing = 
            HandPoseUtils.MiddleFingerCurl(hand) > grabThreshold &&
            HandPoseUtils.RingFingerCurl(hand) > GrabThreshold &&
            HandPoseUtils.PinkyFingerCurl(hand) > GrabThreshold &&
            HandPoseUtils.ThumbFingerCurl(hand) > GrabThreshold;
        return grabbing;
    }

    public bool IsNearby(Handedness hand, float distance, TrackedHandJoint joint = TrackedHandJoint.Palm)
    {
        Transform jointTransform = HandJointService.RequestJointTransform(TrackedHandJoint.Palm, hand);

        float d = Vector3.Distance(jointTransform.position, this.gameObject.transform.position);

        return d < distance;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsGrabbing(Handedness, GrabDistanceThreshold, GrabThreshold, TrackTargetPosition))
        {
            UpdateHandPosition(Handedness, TrackTargetPosition);
            UpdateHandOrientation(Handedness, TrackTargetOrientation);
        }
    }
}
