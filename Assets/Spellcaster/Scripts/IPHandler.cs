using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPHandler : InputActionHandler
{
    public TouchScreenKeyboard keyboard;
    public GameObject GestureListenerObject;
    private GestureListener gestureListener;

    public void Start()
    {
        base.Start();
        keyboard = null;
        gestureListener = GestureListenerObject.GetComponent<GestureListener>();
    }

    public void ToggleSystemKeyboard()
    {
        Debug.Log("Toggling Keyboard");
        if (keyboard != null)
        {
            keyboard = null;
        }
        else
        {
            keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (keyboard != null)
        {
            gestureListener.SetFlaskIPAddress(keyboard.text);
            // Do stuff with keyboardText
        }
    }
}
