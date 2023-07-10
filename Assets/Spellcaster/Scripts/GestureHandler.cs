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
    //[SerializeField]
    public List<Gesture> Gestures;

    /// <summary>
    /// Given the timeline of some poses, determine if there is a gesture being executed.
    /// </summary>
    /// <param name="timeline">The timeline of the poses executed stored in recent order. The most recent elements will be in the beginning with the last events happening at the end of the list.</param>
    /// <returns></returns>
    public Gesture GetGesture(PoseTimeline timeline, float WildcardFactor, Handedness hand = Handedness.Any)
    {
        if (timeline == null)
        {
            return null;
        }
        // If we want hand specific gestures, get them
        List<PoseTimelineObject> timelineObjects = timeline.Timeline;

        // Only consider gestures which even have all of the poses
        IEnumerable<Pose> availablePoses = timelineObjects.Select(x => x.Pose).ToList().Distinct();
        List<Gesture> filteredGestures = Gestures;//.Where(x => x.Requirements.PoseRequirements.All(y => availablePoses.Contains(y.Pose)));
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
            for(int i = 0; i < timelineObjects.Count; i++)
            {
                PoseTimelineObject timeDurationEvent = timelineObjects[i];
                PoseRequirement requirement = requirements[currRequirement];

                // If the current pose is none, thats fine. Move on.
                // If the previous pose was found and it was able to get all of its requirements, do not worry if we find that pose again immediately after.
                if (timeDurationEvent.Pose == Pose.None || (previousPose != Pose.None && previousPose == timeDurationEvent.Pose))
                {
                    continue;
                }
                // We found our pose on the correct hand, so count how long it was held for and check if the requirement was met.
                else if (timeDurationEvent.Pose == requirement.Pose && (hand != Handedness.Any && requirement.Hand == hand))
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
                else if (timeDurationEvent.Duration <= WildcardFactor)
                {
                    // If our current pose found is not a match, still potentially wait for a time to see if we accidentally predicted a wrong intermediate pose
                    continue;
                }
                else if(timeDurationEvent.Pose != requirement.Pose && requirement.Pose != Pose.None && (hand == Handedness.Any && requirement.Hand != hand))
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
    public Gesture GetGesture(PoseTimelineMap timelineMap, float WildcardFactor)
    {
        throw new NotImplementedException();
        List<Handedness> hands = timelineMap.GetTrackedHands().ToList();
        foreach (var gesture in Gestures)
        {
            // Start in reverse order of the requirements to see if the gesture has been executed.
            List<PoseRequirement> requirements = gesture.Requirements.PoseRequirements;
            int totalRequirements = requirements.Count - 1;
            int currRequirement = totalRequirements;
            float currRequirementTime = 0.0f;
            List<bool> requirementSatisfied = Enumerable.Repeat(false, requirements.Count).ToList();

            // Create list of previous poses for the hands.
            Dictionary<Handedness, Pose> previousPose = new Dictionary<Handedness, Pose>();
            previousPose.AddRange(hands.Select(x => new KeyValuePair<Handedness, Pose>(x, Pose.None)));


            Dictionary<Handedness, PoseTimelineObject> visited = new Dictionary<Handedness, PoseTimelineObject>();
            visited.AddRange(hands.Select(x => new KeyValuePair<Handedness, PoseTimelineObject>(x, new PoseTimelineObject())));

            DateTime threshold = DateTime.Now;
            // Start at the end of the pose requirements and check if those poses occured
            for(int i = requirements.Count - 1; i >= 0; i++)
            {
                PoseRequirement requirement = requirements[i];
                Handedness handRequired = requirement.Hand;
                Pose poseRequired = requirement.Pose;
                // Check if the hand 

                PoseTimeline timelineForHand = timelineMap.GetPoseTimeline(handRequired);
                timelineForHand.GetPoseTimelineObjectsBeforeTimestamp(threshold);
            }

        }
        return null;
    }

    public Dictionary<string,List<string>> GetGestureCombos()
    {
        Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();
        foreach (var gesture in Gestures)
        {
            result.Add(gesture.name,gesture.Requirements.PoseRequirements.Select(x => Enum.GetName(typeof(Pose), x.Pose)).ToList());
        }

        return result;
    }
}
