using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class PoseTimelineMap
{
    private Dictionary<Handedness, PoseTimeline> Timelines { get; set; }

    public PoseTimelineMap()
    {
        Timelines = new Dictionary<Handedness, PoseTimeline>();
    }

    public IEnumerable<Handedness> GetTrackedHands()
    {
        return Timelines.Keys;
    }

    public void AddPoseEvent(PoseEvent poseEvent)
    {
        PoseTimeline timeline = null;
        // Find the timelines corresponding to the hand and update its data
        if (!Timelines.ContainsKey(poseEvent.Hand))
        {
            Timelines.Add(poseEvent.Hand, new PoseTimeline());
        }
        timeline = Timelines[poseEvent.Hand];
        timeline.AddPoseEvent(poseEvent);
    }

    /// <summary>
    /// Clears the timeline for the specified hand if it exists.
    /// </summary>
    /// <param name="hand"></param>
    public void ClearTimeline(Handedness hand)
    {
        if (!Timelines.ContainsKey(hand))
        {
            return;
        }

        Timelines[hand].ClearTimeline();
    }

    /// <summary>
    /// Clears the timelines for all hands.
    /// </summary>
    /// <param name="hand"></param>
    public void ClearTimelines()
    {
        foreach(var hand in  Timelines.Keys)
        {
            Timelines[hand].ClearTimeline();
        }
    }

    /// <summary>
    /// Returns the latest pose for a given hand if it exists
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    public Pose LatestPose(Handedness hand)
    {
        if (!Timelines.ContainsKey(hand))
        {
            return Pose.None;
        }

        return Timelines[hand].LatestPose();
    }

    /// <summary>
    /// Returns the latest poses of all currently tracked hand timelines.
    /// </summary>
    /// <returns></returns>
    public Dictionary<Handedness, Pose> LatestPoses()
    {
        Dictionary<Handedness, Pose> latest = new Dictionary<Handedness, Pose>();
        foreach (var hand in Timelines.Keys)
        {
            latest.Add(hand, Timelines[hand].LatestPose());
        }
        return latest;
    }

    public PoseTimeline GetPoseTimeline(Handedness hand)
    {
        if(Timelines.ContainsKey(hand))
        {
            return Timelines[hand];
        }
        return null;
    }

    public PoseTimelineObject LatestPoseTimeline(Handedness Hand)
    {
        if (!Timelines.ContainsKey(Hand))
        {
            return null;
        }

        return Timelines[Hand].LatestPoseTimeline();
    }
}