using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class GestureStateMachine
{
    public PoseTimelineMap PoseTimelineMap { get; }

    /// <summary>
    /// The max number of poses to keep track of
    /// </summary>
    private int _maxHistory;
    public GestureStateMachine(int maxHistory = 1000)
    {
        PoseTimelineMap = new PoseTimelineMap();
        _maxHistory = maxHistory;
    }

    public Dictionary<Handedness,Pose> GetMostRecentPoses()
    {
        return PoseTimelineMap.LatestPoses();
    }

    public Pose GetMostRecentPose(Handedness hand)
    {
        return PoseTimelineMap.LatestPose(hand);
    }

    /// <summary>
    /// Apply the given pose to our history
    /// </summary>
    /// <param name="p">The event to apply to our history.</param>
    /// <returns></returns>
    public void ApplyPose(PoseEvent p)
    {
        PoseTimelineMap.AddPoseEvent(p);
    }
}
