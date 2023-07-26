using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildcardHandler : InputActionHandler
{
    public void ToggleWildcard()
    {
        Debug.Log("Toggling Wildcard");
        GameObject gManager = GameObject.Find("GestureManager");
        if (gManager != null)
        {
            gManager.GetComponent<GestureListener>().ToggleWildcard();
        }
    }
}
