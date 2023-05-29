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
    public List<GestureCallback> GestureCallbacks;
    Handedness _rightHand;
    Handedness _leftHand;
    GestureHandler _gestureHandler;
    PoseHandler _poseHandler;
    GestureStateMachine _stateMachine;
    private IMixedRealityHandJointService handJointService;

    // Start is called before the first frame update
    void Start()
    {
        _rightHand= Handedness.Right;
        _leftHand = Handedness.Left;
        _gestureHandler = new GestureHandler();
        _poseHandler = new PoseHandler();
        _stateMachine = new GestureStateMachine();
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        PoseEvent rh = new PoseEvent()
        {
            Hand = _rightHand,
            Pose = _poseHandler.GetPose(_rightHand),
            TimeDelta = delta,
            TimeStamp = DateTime.Now
        };

        PoseEvent lh = new PoseEvent()
        {
            Hand = _leftHand,
            Pose = _poseHandler.GetPose(_leftHand),
            TimeDelta = delta,
            TimeStamp = DateTime.Now
        };

        if(lh.Pose != Pose.None && lh.Pose != Pose.Unknown)
        {
            Debug.Log("Left hand pose: " + Enum.GetName(typeof(Pose), lh.Pose));
        }
        if (rh.Pose != Pose.None && rh.Pose != Pose.Unknown)
        {
            Debug.Log("Right hand pose: " + Enum.GetName(typeof(Pose), rh.Pose));
        }

        _stateMachine.ApplyPose(_rightHand, rh);
        _stateMachine.ApplyPose(_leftHand, lh);
        List<PoseEvent> lhHistory = _stateMachine.GetPoseHistory(_leftHand);
        List<PoseEvent> rhHistory = _stateMachine.GetPoseHistory(_rightHand);
        List<float> th = _stateMachine.GetTimeHistory();

        //List<PoseEvent> ph = _stateMachine.GetPoseHistory();

        Gesture gestureExecuted = _gestureHandler.GetGesture(lhHistory, rhHistory, th);
        GestureCallback callback = GestureCallbacks.Find(x => x.Gesture == gestureExecuted);
        if(callback != null)
        {
            Debug.Log($"Invoking {callback.Gesture} callback");
            callback.GestureOnStart.Invoke();
        }

        if (gestureExecuted != Gesture.None)
        {
            _stateMachine.ClearHistory();
        }
    }
}
