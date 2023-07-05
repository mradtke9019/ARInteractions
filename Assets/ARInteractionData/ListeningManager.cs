using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListeningManager : InputActionHandler
{
    public void StartListening()
    {
        GameObject gManager = GameObject.Find("GestureManager");
        if (gManager != null)
        {
            gManager.GetComponent<GestureListener>().SetApplicationMode(ApplicationMode.Listening);
        }
    }
}
