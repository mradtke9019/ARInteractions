using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class GestureHandler
{
    public Gesture GetGesture(List<PoseEvent> poseHistoryLH, List<PoseEvent> poseHistoryRH, List<float> timeHistory)
    {
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
            return Gesture.None;
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
                if(p == Pose.FistPalmUp)
                {
                    return Gesture.Fireball;
                }
                if (p == Pose.FistPalmHorizontal)
                {
                    return Gesture.Fireball;
                }
            }
        }

        return Gesture.None;
    }
}
