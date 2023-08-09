using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReleaseManager : InputActionHandler
{
    public void StartRelease()
    {
        GameObject gManager = GameObject.Find("GestureManager");
        if (gManager != null)
        {
            gManager.GetComponent<GestureListener>().SetApplicationMode(ApplicationMode.Release);
        }
    }
}
