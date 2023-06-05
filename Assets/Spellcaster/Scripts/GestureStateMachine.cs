using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class GestureStateMachine
{
    private PoseTimeline _timeline;
    private List<PoseEvent> _poseHistory;
    //private List<PoseEvent> _leftHandPoseHistory;
    //private List<PoseEvent> _rightHandPoseHistory;
    //private List<float> _timeDeltaHistory;

    /// <summary>
    /// The max number of poses to keep track of
    /// </summary>
    private int _maxHistory;
    public GestureStateMachine(int maxHistory = 1000)
    {
         _timeline = new PoseTimeline();
        _poseHistory = new List<PoseEvent>(maxHistory);
        //_leftHandPoseHistory = new List<PoseEvent>(maxHistory);
        //_rightHandPoseHistory = new List<PoseEvent>(maxHistory);
        //_timeDeltaHistory = new List<float>();
        _maxHistory = maxHistory;
    }

    /// <summary>
    /// Return the history of the hands.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    public List<PoseEvent> GetPoseHistory()
    {
        return _poseHistory.ToList();
        //return _poseHistory.Where(x => x.Hand == hand).ToList();
/*        if (hand == Handedness.Right)
            return _leftHandPoseHistory;
        else
            return _rightHandPoseHistory;*/
    }

    /// <summary>
    /// Return the timeline of the poses.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    public PoseTimeline GetPoseTimeline()
    {
        return _timeline;
    }

    public Pose GetMostRecentPose()
    {
        return _timeline.LatestPose();
    }

    /// <summary>
    /// Return the time history of the poses.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
/*    public List<float> GetTimeHistory()
    {
        return _timeDeltaHistory;
    }
*/
    /// <summary>
    /// Apply the given pose to our history
    /// </summary>
    /// <param name="p">The event to apply to our history.</param>
    /// <returns></returns>
    public void ApplyPose(PoseEvent p)
    {
        _poseHistory.Add(p);
        _timeline.AddPoseEvent(p);
/*        switch (hand)
        {
            case Handedness.Left:
                _leftHandPoseHistory.Add(p);
                break;
            case Handedness.Right:
                _rightHandPoseHistory.Add(p);
                break;
        }*/
        //_timeDeltaHistory.Add(Time.deltaTime);

        //Cull list to only contain the amount of data we want.
        /*        if(_timeDeltaHistory.Count > _maxHistory)
                {
                    _timeDeltaHistory = _timeDeltaHistory.Skip(Math.Max(0, _timeDeltaHistory.Count() - _maxHistory)).ToList();
                }*/
        if (_poseHistory.Count > _maxHistory)
        {
            _poseHistory = _poseHistory.Skip(Math.Max(0, _poseHistory.Count() - _maxHistory)).ToList();
        }
        /*if (_leftHandPoseHistory.Count > _maxHistory)
        {
            _leftHandPoseHistory = _leftHandPoseHistory.Skip(Math.Max(0, _leftHandPoseHistory.Count() - _maxHistory)).ToList();
        }
        if (_rightHandPoseHistory.Count > _maxHistory)
        {
            _rightHandPoseHistory = _rightHandPoseHistory.Skip(Math.Max(0, _rightHandPoseHistory.Count() - _maxHistory)).ToList();
        }*/
    }

    public void ClearHistory()
    {
        _poseHistory = new List<PoseEvent>();
        _timeline = new PoseTimeline();
/*        _leftHandPoseHistory = new List<PoseEvent>();
        _rightHandPoseHistory = new List<PoseEvent>();
        _timeDeltaHistory = new List<float>();*/
    }
}
