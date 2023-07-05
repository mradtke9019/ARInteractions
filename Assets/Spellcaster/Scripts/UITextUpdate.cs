using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITextUpdate : MonoBehaviour
{
    public GameObject GestureListenerTarget;
    private GestureListener gestureListener;
    private Text textObject;
    private void Start()
    {
        gestureListener = this.GetComponent<GestureListener>();
        textObject = GestureListenerTarget.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = gestureListener.GetDebugInfo();
    }
}
