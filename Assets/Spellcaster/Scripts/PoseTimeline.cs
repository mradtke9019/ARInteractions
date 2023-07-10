using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class PoseTimeline
{
    public List<PoseTimelineObject> Timeline { get; set; }  
    private int _maxHistory;
    public PoseTimeline()
    {
        Timeline = new List<PoseTimelineObject>();
    }
    public PoseTimeline(List<PoseTimelineObject> timeline)
    {
        Timeline = timeline;
    }

    public void AddPoseEvent(PoseEvent poseEvent)
    {
        if (Timeline.Count == 0)
        {
            Timeline.Add(new PoseTimelineObject()
            {
                Pose = poseEvent.Pose,
                Duration = poseEvent.TimeDelta,
                Start = poseEvent.TimeStamp
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
                Duration = poseEvent.TimeDelta,
                Start = poseEvent.TimeStamp
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

    public PoseTimeline GetPoseTimelineObjectsAfterTimestamp(DateTime timeStamp)
    {
        return new PoseTimeline(Timeline.Where(x => x.Start <= timeStamp).ToList());
    }
    /// <summary>
    /// Returns timeline where the poses have both started and ended strictly before a time. Considers each pose timeline objects
    /// duration of execution.
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public PoseTimeline GetPoseTimelineObjectsBeforeTimestamp(DateTime timeStamp)
    {
        return new PoseTimeline(Timeline.Where(x =>
        {
            DateTime time = x.Start;
            time.AddSeconds(x.Duration);
            return time < timeStamp;

        }).ToList());
    }
}
