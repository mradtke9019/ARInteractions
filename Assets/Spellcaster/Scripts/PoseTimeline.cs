using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using Microsoft.MixedReality.Toolkit.Utilities;
public class PoseTimeline
{
    public List<PoseTimelineObject> Timeline { get; set; }  
    public Dictionary<Handedness, List<PoseTimelineObject>> Timelines { get; set; }  
    private int _maxHistory;
    public PoseTimeline()
    {
        Timeline = new List<PoseTimelineObject>();
        Timelines = new Dictionary<Handedness, List<PoseTimelineObject>>();
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

        // Find the timelines corresponding to the hand and update its data
        if(!Timelines.ContainsKey(poseEvent.Hand))
        {
            Timelines.Add(poseEvent.Hand, new List<PoseTimelineObject>());
        }
        List<PoseTimelineObject> timeline = Timelines[poseEvent.Hand];
        if (timeline.Count == 0)
        {
            timeline.Add(new PoseTimelineObject()
            {
                Pose = poseEvent.Pose,
                Duration = poseEvent.TimeDelta
            });
        }

        else if (timeline.First().Pose == poseEvent.Pose)
        {
            timeline.First().Duration += poseEvent.TimeDelta;
        }
        else
        {
            timeline.Insert(0, new PoseTimelineObject()
            {
                Pose = poseEvent.Pose,
                Duration = poseEvent.TimeDelta
            });
        }
    }

    public void ClearTimeline()
    {
        Timeline.Clear();
        Timelines.Clear();
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

    public Dictionary<Handedness, Pose> LatestPoses()
    {
        Dictionary<Handedness, Pose> latest = new Dictionary<Handedness, Pose>();
        foreach(var hand in Timelines.Keys)
        {
            PoseTimelineObject obj = Timelines[hand].FirstOrDefault();
            latest.Add(hand, obj != null ? obj.Pose : Pose.None);
        }
        return latest;
    }

    public PoseTimelineObject LatestPoseTimeline(Handedness Hand)
    {
        if(!Timelines.ContainsKey(Hand))
        {
            return null;
        }
        PoseTimelineObject obj = Timelines[Hand].FirstOrDefault();
        return obj;
    }

    public PoseTimelineObject LatestPoseTimeline()
    {
        PoseTimelineObject obj = Timeline.FirstOrDefault();
        return obj;
    }
}
