using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

[Serializable]
public class GestureHandler
{
    [SerializeField]
    public List<Gesture> Gestures;

    /// <summary>
    /// Given the timeline of some poses, determine if there is a gesture being executed.
    /// </summary>
    /// <param name="timeline">The timeline of the poses executed stored in recent order. The most recent elements will be in the beginning with the last events happening at the end of the list.</param>
    /// <returns></returns>
    public Gesture GetGesture(PoseTimeline timeline)
    {
        // Only consider gestures which even have all of the poses
        var availablePoses = timeline.Timeline.Select(x => x.Pose).ToList().Distinct();
        var filteredGestures = Gestures.Where(x => x.Requirements.PoseRequirements.All(y => availablePoses.Contains(y.Pose)));
        foreach (var gesture in filteredGestures)
        {
            // Start in reverse order of the requirements to see if the gesture has been executed.
            List<PoseRequirement> requirements = gesture.Requirements.PoseRequirements;
            int totalRequirements = requirements.Count - 1;
            int currRequirement = totalRequirements;
            float currRequirementTime = 0.0f;
            List<bool> requirementSatisfied = Enumerable.Repeat(false, requirements.Count).ToList();
            Pose previousPose = Pose.None;
            // Check if the events in the timeline match the requirements of the current gesture.
            for(int i = 0; i < timeline.Timeline.Count; i++)
            {
                PoseTimelineObject timeDurationEvent = timeline.Timeline[i];
                PoseRequirement requirement = requirements[currRequirement];

                // If the current pose is none, thats fine. Move on.
                // If the previous pose was found and it was able to get all of its requirements, do not worry if we find that pose again immediately after.
                if (timeDurationEvent.Pose == Pose.None || (previousPose != Pose.None && previousPose == timeDurationEvent.Pose))
                {
                    continue;
                }
                // We found our pose, so count how long it was held for and check if the requirement was met.
                else if (timeDurationEvent.Pose == requirement.Pose)
                {
                    currRequirementTime += timeDurationEvent.Duration;
                    if(currRequirementTime >= requirement.Duration)
                    {
                        requirementSatisfied[currRequirement] = true;
                        currRequirement--;
                        previousPose = requirement.Pose;
                        currRequirementTime = 0.0f;
                    }
                }
                else if(timeDurationEvent.Pose != requirement.Pose && requirement.Pose != Pose.None)
                {
                    // Our current data is a mismatch, so need to restart search since our data was interrupted by a pose not part of the current gesture.
                    currRequirement = totalRequirements;
                    requirementSatisfied = Enumerable.Repeat(false, requirements.Count).ToList();
                    previousPose = Pose.None;
                    currRequirementTime = 0.0f;
                }

                // If the current timeline satisfies the requirements of the current gesture, return it.
                if (requirementSatisfied.All(x => x))
                {
                    return gesture;
                }
            }
        }
        return null;
    }
    /// <summary>
    /// Given the history of some poses, determine if there is a gesture being executed.
    /// </summary>
    /// <param name="history">The history stored in </param>
    /// <returns></returns>
    public Gesture GetGesture(List<PoseEvent> history)
    {
        foreach(var Gesture in Gestures)
        {
            List<PoseRequirement> requirements = Gesture.Requirements.PoseRequirements;
            bool[] requirementsSatisfied = new bool[requirements.Count];
            requirements.Reverse();
            history.Reverse();
            int idx = history.Count - 1;
            // Work our way backwards from the end requirements to the beginning to see if we have executed a gesture.
            foreach (var poseRequirement in requirements)
            {
                float duration = 0.0f;
                Handedness hand = poseRequirement.Hand;
                List<PoseEvent> filteredHistory = history.Where(x => x.Hand == hand).ToList();
                //filteredHistory.
                // Check that only the current pose has been executed for at least the specified amount of time in the correct order.
                for(int i = 0; i < filteredHistory.Count; i++)
                {
                    PoseEvent currEvent = filteredHistory[i];
                    // This is not the pose. Move on
                    if (currEvent.Pose != poseRequirement.Pose && currEvent.Pose != Pose.None && currEvent.Pose != Pose.Unknown)
                    {
                        continue;
                    }
                    else if (currEvent.Pose == poseRequirement.Pose)
                    {
                        duration += currEvent.TimeDelta;
                    }


                    // We have met the duration requirement for this pose in the gesture, so we can check if the next pose has been executed.
                    if (duration >= poseRequirement.Duration)
                    {
                        // Pick up where this pose has left off
                        break;
                    }
                }
                foreach (var currEvent in filteredHistory)
                {
                }


            }
        }

        return null;/*
        int lastPoses = 0;
        int idx = timeHistory.Count - 1;
        float currentTime = 0.0f;
        float timeCutoff = 3.0f;

        while(idx>= 0 && currentTime < timeCutoff)
        {
            currentTime += timeHistory[idx];


            lastPoses++;
            idx--;
        }

        if(lastPoses == 0 || currentTime < timeCutoff)
        {
            return null;
        }
        //List<PoseEvent> poseHistoryRH = poseHistory.Where(x => x.Hand == Handedness.Right).ToList();
        //List<PoseEvent> poseHistoryLH = poseHistory.Where(x => x.Hand == Handedness.Left).ToList();


        List<PoseEvent> filteredPosesRH = poseHistoryRH.Skip(Math.Max(0, poseHistoryRH.Count() - lastPoses)).ToList();
        List<PoseEvent> filteredPosesLH = poseHistoryLH.Skip(Math.Max(0, poseHistoryLH.Count() - lastPoses)).ToList();

        // Now that we have the last data of the items, check and see if there is a gesture being executed from these poses
        foreach(Pose p in Enum.GetValues(typeof(Pose)))
        {
            List<PoseEvent> pEvents = filteredPosesLH.FindAll(x => x.Pose == p).ToList();
            float sum = pEvents.Sum(x => x.TimeDelta);
            if (sum >= 1.0f)
            {
                *//*if(p == Pose.FistPalmUp)
                {
                    return Gestures.Find(x => x._Gesture);
                }
                if (p == Pose.FistPalmHorizontal)
                {
                    return GestureType.Fireball;
                }*//*
            }
        }

        return null;*/
    }
}
