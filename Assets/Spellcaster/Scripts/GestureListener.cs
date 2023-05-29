using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

public class GestureListener : MonoBehaviour
{
    [SerializeField]
    GestureHandler GestureHandler;

    Handedness _rightHand;
    Handedness _leftHand;

    PoseHandler _poseHandler;
    GestureStateMachine _stateMachine;
    private IMixedRealityHandJointService handJointService;

    // Start is called before the first frame update
    void Start()
    {
        _rightHand= Handedness.Right;
        _leftHand = Handedness.Left;
        //GestureHandler = new GestureHandler();
        _poseHandler = new PoseHandler();
        _stateMachine = new GestureStateMachine();
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        DateTime timeStamp = DateTime.Now;
        PoseEvent rh = new PoseEvent()
        {
            Hand = _rightHand,
            Pose = _poseHandler.GetPose(_rightHand),
            TimeDelta = delta,
            TimeStamp = timeStamp
        };

        PoseEvent lh = new PoseEvent()
        {
            Hand = _leftHand,
            Pose = _poseHandler.GetPose(_leftHand),
            TimeDelta = delta,
            TimeStamp = timeStamp
        };

/*        if(lh.Pose != Pose.None && lh.Pose != Pose.Unknown)
        {
            Debug.Log("Left hand pose: " + Enum.GetName(typeof(Pose), lh.Pose));
        }*/
        if (rh.Pose != Pose.None && rh.Pose != Pose.Unknown)
        {
            Debug.Log("Right hand pose: " + Enum.GetName(typeof(Pose), rh.Pose));
        }

        _stateMachine.ApplyPose(rh);
        //_stateMachine.ApplyPose(lh);

        //List<PoseEvent> lhHistory = _stateMachine.GetPoseHistory(_leftHand);
        //List<PoseEvent> rhHistory = _stateMachine.GetPoseHistory(_rightHand);
        //List<float> th = _stateMachine.GetTimeHistory();

        //List<PoseEvent> ph = _stateMachine.GetPoseHistory();

        //GestureType gestureExecuted = _gestureHandler.GetGesture(lhHistory, rhHistory, th);
        List<PoseEvent> history = _stateMachine.GetPoseHistory();
        PoseTimeline timeline = _stateMachine.GetPoseTimeline();
        Gesture g = GestureHandler.GetGesture(timeline);
        //Gesture g = _gestureHandler.GetGesture(history);
        if (g != null)
        {
            timeline.ClearTimeline();
            GestureCallback callback = g.GestureCallback;
            Debug.Log($"Gesture executed: {g.name}.");
            callback.GestureOnStart.Invoke();
        }
    }
}
