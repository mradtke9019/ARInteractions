using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ContinuityType
{
    Continuous,
    Finite
}

[Serializable]
public class Continuity 
{
    [SerializeField]
    ContinuityType type;
    [SerializeField]
    float Duration;
}
