using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPTextUpdate : MonoBehaviour
{
    public GameObject GestureListenerTarget;
    private GestureListener gestureListener;
    private TextMeshProUGUI tmp;
    private void Start()
    {
        tmp = this.GetComponent<TextMeshProUGUI>();
        gestureListener = GestureListenerTarget.GetComponent<GestureListener>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = gestureListener.GetDebugInfo();
    }
}
