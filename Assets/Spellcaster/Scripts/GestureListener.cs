using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Threading.Tasks;
using System.Linq;

public class GestureListener : MonoBehaviour
{
    [SerializeField]
    GestureHandler GestureHandler;
    public MLModel Model = MLModel.KNN;
    public ApplicationMode ApplicationMode = ApplicationMode.Listening;
    public bool UseWildcard = true;
    public float WildcardFactor = 0.15f;
    public string FlaskIPAddress = "192.168.0.101";

    PoseHandler _poseHandler;
    GestureStateMachine _stateMachine;

    // Gesture that is being continuously executed
    private Dictionary<Handedness, Gesture> CurrentlyExecutedGestures;

    Handedness _rightHand;
    Handedness _leftHand;
    private IMixedRealityHandJointService handJointService;

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
        CurrentlyExecutedGestures = new Dictionary<Handedness, Gesture>();
        CurrentlyExecutedGestures.Add(_rightHand, null);
        CurrentlyExecutedGestures.Add(_leftHand, null);

        gestureNameCombos = GestureHandler.GetGestureCombos();
    }

    // Update is called once per frame
    void Update()
    {
        if(ApplicationMode != ApplicationMode.Listening)
        {
            return;
        }

        float delta = Time.deltaTime;
        DateTime timeStamp = DateTime.Now;

        // Get the poses for both hands
        List<Pose> poses = _poseHandler.GetPoses();

        PoseEvent lh = new PoseEvent()
        {
            Hand = _leftHand,
            Pose = poses[0],
            TimeDelta = delta,
            TimeStamp = timeStamp
        };

        PoseEvent rh = new PoseEvent()
        {
            Hand = _rightHand,
            Pose = poses[1],
            TimeDelta = delta,
            TimeStamp = timeStamp
        };


        _stateMachine.ApplyPose(rh);
        _stateMachine.ApplyPose(lh);


        PoseTimelineMap timelineMap = _stateMachine.PoseTimelineMap;


        // If I am in the middle of a continuosly executed gesture, do not consider executing the same gesture again.
        // Only allow it to execute the gesture if we have deliberately stopped this continous gesture
        //bool newGestureExecuted = g != null && CurrentlyExecutedGesture != null && CurrentlyExecutedGesture.name != g.name;
        // We are done executing the gesture if the currently executed gesture's last pose does not match our current pose

        foreach(var hand in timelineMap.GetTrackedHands())
        {
            // Consider if one of the hands is currently part of a continuous gesture. If so, we should not consider that hand for future gestures.
            Gesture g = GestureHandler.GetGesture(timelineMap.GetPoseTimeline(hand), UseWildcard ? WildcardFactor : 0.0f, hand);


            Gesture CurrentlyExecutedGesture = CurrentlyExecutedGestures[hand];
            bool doneExecutingExistingGesture =
                CurrentlyExecutedGesture != null &&
                (CurrentlyExecutedGesture.FinalPoseHand() == hand &&
                CurrentlyExecutedGesture.GetFinalPose() != timelineMap.LatestPose(hand));


            // The gesture is no longer be executed or a new gesture is being started. Either way, start our on end functions.
            if (doneExecutingExistingGesture /*|| newGestureExecuted*/)
            {
                CurrentlyExecutedGestures[hand].GestureCallback.GestureOnEnd.Invoke();
                CurrentlyExecutedGestures[hand] = null;
            }

            //Gesture g = _gestureHandler.GetGesture(history);
            if (g != null)
            {
                // Since we just executed a gesture, clear the timeline history for all the hands tracked.
                timelineMap.ClearTimelines();
                GestureCallback callback = g.GestureCallback;
                Debug.Log($"Gesture executed: {g.name}.");
                callback.GestureOnStart.Invoke();
                if (g.Continuity.type == ContinuityType.Continuous)
                {
                    CurrentlyExecutedGestures[g.FinalPoseHand()] = g;
                }
                else if (g.Continuity.type == ContinuityType.Finite && !g.GestureCallback.GestureOnEnd.IsUnityNull() && !g.GestureCallback.GestureOnEnd.IsNull() && g.Continuity.Duration > 0)
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
    }

    public void SetModeRecording()
    {
        this.ApplicationMode = ApplicationMode.Recording;
    }

    public void SetModeListening()
    {
        this.ApplicationMode = ApplicationMode.Listening;
    }

    public void SetApplicationMode(ApplicationMode mode)
    {
        this.ApplicationMode = mode;
    }
    
    public void ToggleWildcard()
    {
        Debug.Log("Toggling wildcard");
        UseWildcard = !UseWildcard;
    }

    public void SetFlaskIPAddress(string ip)
    {
        Debug.Log("Flask IP set");
        FlaskIPAddress = ip;
        _poseHandler.SetIP(ip);
    }

    public string GetDebugInfo()
    {
        string val = "";
        
        if(ApplicationMode == ApplicationMode.Recording)
        {
            return "In data capture mode." + Environment.NewLine + 
                "Say 'Listen' to being gesture interactions" + Environment.NewLine + 
                "Flask IP: " + FlaskIPAddress + Environment.NewLine +
                "Using Wildcard: " + UseWildcard.ToString();
        }

        // Currently consider each hand by itself for executing gestures
        foreach (var hand in _stateMachine.PoseTimelineMap.GetTrackedHands())
        {
            PoseTimelineObject latest = _stateMachine.PoseTimelineMap.LatestPoseTimeline(hand);
            if(latest == null)
            {
                continue;
            }
            string name = $"{Enum.GetName(typeof(Handedness), hand)}: {Enum.GetName(typeof(Pose), latest.Pose)}, {latest.Duration}" + Environment.NewLine;
            val += name;
        }

        val += "Combos:" + Environment.NewLine;
        foreach (var key in gestureNameCombos.Keys)
        {
            val += key + ": " + string.Join(", " , gestureNameCombos[key].Select(x => x)) + Environment.NewLine;
        }

        return val;
    }
}
