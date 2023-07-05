using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingManager : InputActionHandler
{
    public void StartRecording()
    {
        GameObject gManager = GameObject.Find("GestureManager");
        if (gManager != null)
        {
            gManager.GetComponent<GestureListener>().SetApplicationMode(ApplicationMode.Recording);
        }
    }

}
