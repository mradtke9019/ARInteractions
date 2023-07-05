using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Threading;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Threading.Tasks;
using System.Linq;

public class GestureListener : MonoBehaviour
{
    [SerializeField]
    GestureHandler GestureHandler;
    public MLModel Model = MLModel.KNN;
    // Gesture that is being continuously executed
    private Gesture CurrentlyExecutedGesture;

    Handedness _rightHand;
    Handedness _leftHand;

    PoseHandler _poseHandler;
    GestureStateMachine _stateMachine;
    private IMixedRealityHandJointService handJointService;
    private ApplicationMode mode = ApplicationMode.Listening;

    private Dictionary<string, List<string>> gestureNameCombos;

    // Start is called before the first frame update
    void Start()
    {
        _rightHand= Handedness.Right;
        _leftHand = Handedness.Left;
        //GestureHandler = new GestureHandler();
        _poseHandler = new PoseHandler(Model);
        _stateMachine = new GestureStateMachine();
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        CurrentlyExecutedGesture = null;
        gestureNameCombos = GestureHandler.GetGestureCombos();
    }

    // Update is called once per frame
    void Update()
    {
        if(mode != ApplicationMode.Listening)
        {
            return;
        }

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
        if (rh.Pose != Pose.None)
        {
            Debug.Log("Right hand pose: " + Enum.GetName(typeof(Pose), rh.Pose));
        }

        _stateMachine.ApplyPose(rh);
        //_stateMachine.ApplyPose(lh);

        PoseTimeline timeline = _stateMachine.GetPoseTimeline();
        Gesture g = GestureHandler.GetGesture(timeline);


        // If I am in the middle of a continuosly executed gesture, do not consider executing the same gesture again.
        // Only allow it to execute the gesture if we have deliberately stopped this continous gesture
        // The gesture is no longer be executed or a new gesture is being started. Either way, start our on end functions.
        bool newGestureExecuted = g != null && CurrentlyExecutedGesture != null && CurrentlyExecutedGesture.name != g.name;
        bool doneExecutingExistingGesture = CurrentlyExecutedGesture != null && CurrentlyExecutedGesture.GetFinalPose() != timeline.LatestPose();
        if (newGestureExecuted || doneExecutingExistingGesture)
        {
            CurrentlyExecutedGesture.GestureCallback.GestureOnEnd.Invoke();
            CurrentlyExecutedGesture = null;
        }

        //Gesture g = _gestureHandler.GetGesture(history);
        if (g != null)
        {
            timeline.ClearTimeline();
            GestureCallback callback = g.GestureCallback;
            Debug.Log($"Gesture executed: {g.name}.");
            callback.GestureOnStart.Invoke();
            if(g.Continuity.type == ContinuityType.Continuous)
            {
                CurrentlyExecutedGesture = g;
            }
            else if(g.Continuity.type == ContinuityType.Finite && !g.GestureCallback.GestureOnEnd.IsUnityNull() && !g.GestureCallback.GestureOnEnd.IsNull() && g.Continuity.Duration > 0)
            {
                // Wait the specified amount of time to execute the on end functions
                Action a = () =>
                {
                    int delay = (int)g.Continuity.Duration * 1000;
                    Debug.Log($"Waiting {delay} seconds for gesture on end invocation.");
                    Task.Delay(delay);
                    g.GestureCallback.GestureOnEnd.Invoke();
                };
                a.Invoke();
            }
        }

    }
    public void SetModeRecording()
    {
        this.mode = ApplicationMode.Recording;
    }

    public void SetModeListening()
    {
        this.mode = ApplicationMode.Listening;
    }

    public void SetApplicationMode(ApplicationMode mode)
    {
        this.mode = mode;
    }

    public string GetDebugInfo()
    {
        string val = "";
        
        if(mode == ApplicationMode.Recording)
        {
            return "In recording mode." + Environment.NewLine + 
                "Say 'Listen' to being gesture interactions";
        }


        PoseTimeline timeline = _stateMachine.GetPoseTimeline();
        PoseTimelineObject p = timeline.LatestPoseTimeline();
        if (p == null)
            return "Null";
        string name = Enum.GetName(typeof(Pose), p.Pose);
        val = name + ": " + p.Duration;
        val += Environment.NewLine;
        val += "Combos:" + Environment.NewLine;
        foreach (var key in gestureNameCombos.Keys)
        {
            val += key + ": " + string.Join(", " , gestureNameCombos[key].Select(x => x)) + Environment.NewLine;
        }

        return val;
    }
}
