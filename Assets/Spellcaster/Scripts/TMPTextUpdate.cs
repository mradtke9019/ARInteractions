using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TMPTextUpdate : MonoBehaviour
{
    public GameObject Target;
    private GestureListener gestureListener;
    private TextMeshProUGUI tmp;
    private void Start()
    {
        tmp = this.GetComponent<TextMeshProUGUI>();
        gestureListener = Target.GetComponent<GestureListener>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = gestureListener.GetDebugInfo();
    }
}
