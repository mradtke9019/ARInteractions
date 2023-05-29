using Microsoft.MixedReality.Toolkit.Utilities;
using System;

public class PoseEvent
{
    public Handedness Hand { get; set; }
    public Pose Pose { get; set; }
    public float TimeDelta { get; set; }
    public DateTime TimeStamp { get; set; }
}
