using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Microsoft.MixedReality.Toolkit.Utilities;
public class PoseTimeline
{
    public List<PoseTimelineObject> Timeline { get; set; }  
    private int _maxHistory;
    public PoseTimeline()
    {
        Timeline = new List<PoseTimelineObject>();
    }

    public void AddPoseEvent(PoseEvent poseEvent)
    {
        if (Timeline.Count == 0)
        {
            Timeline.Add(new PoseTimelineObject()
            {
                Pose = poseEvent.Pose,
                Duration = poseEvent.TimeDelta
            });
        }

        else if (Timeline.First().Pose == poseEvent.Pose)
        {
            Timeline.First().Duration += poseEvent.TimeDelta;
        }
        else
        {
            Timeline.Insert(0, new PoseTimelineObject()
            {
                Pose = poseEvent.Pose,
                Duration = poseEvent.TimeDelta
            });
        }
    }

    public void ClearTimeline()
    {
        Timeline.Clear();
    }

    public Pose LatestPose()
    {
        PoseTimelineObject obj = Timeline.FirstOrDefault();
        if (obj == null)
        {
            return Pose.None;
        }
        return obj.Pose;
    }

    public PoseTimelineObject LatestPoseTimeline()
    {
        PoseTimelineObject obj = Timeline.FirstOrDefault();
        return obj;
    }
}
