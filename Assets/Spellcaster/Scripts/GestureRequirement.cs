using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GestureRequirement
{
    [SerializeField]
    public List<PoseRequirement> PoseRequirements = new List<PoseRequirement>();
}
