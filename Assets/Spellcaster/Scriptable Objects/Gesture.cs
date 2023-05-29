using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Gesture", menuName = "ScriptableObjects/Gesture")]
public class Gesture : ScriptableObject
{
    public string Label = "";
    //public GestureType _Gesture;
    public Continuity Continuity;
    public GestureRequirement Requirements;
    public GestureCallback GestureCallback;
}
