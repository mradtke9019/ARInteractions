using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GestureCallback
{
    public UnityEvent GestureOnStart;
    public UnityEvent GestureOnEnd;
}
