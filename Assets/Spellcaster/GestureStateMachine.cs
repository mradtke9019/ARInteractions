using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GestureStateMachine
{
    private List<PoseEvent> _poseHistory;
    private List<PoseEvent> _leftHandPoseHistory;
    private List<PoseEvent> _rightHandPoseHistory;
    private List<float> _timeDeltaHistory;
    public GestureStateMachine()
    {
        _poseHistory = new List<PoseEvent>();
        _leftHandPoseHistory = new List<PoseEvent>();
        _rightHandPoseHistory = new List<PoseEvent>();
        _timeDeltaHistory = new List<float>();
    }

    /// <summary>
    /// Return the history of the hands.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    public List<PoseEvent> GetPoseHistory(Handedness hand = Handedness.Any)
    {
        //return _poseHistory.Where(x => x.Hand == hand).ToList();
        if (hand == Handedness.Right)
            return _leftHandPoseHistory;
        else
            return _rightHandPoseHistory;
    }

    /// <summary>
    /// Return the time history of the poses.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    public List<float> GetTimeHistory()
    {
        return _timeDeltaHistory;
    }


    public void ApplyPose(Handedness hand, PoseEvent p)
    {
        _poseHistory.Add(p);

        switch (hand)
        {
            case Handedness.Left:
                _leftHandPoseHistory.Add(p);
                break;
            case Handedness.Right:
                _rightHandPoseHistory.Add(p);
                break;
        }

        _timeDeltaHistory.Add(Time.deltaTime);
    }

    public void ClearHistory()
    {
        _leftHandPoseHistory = new List<PoseEvent>();
        _rightHandPoseHistory = new List<PoseEvent>();
        _timeDeltaHistory = new List<float>();
    }
}
