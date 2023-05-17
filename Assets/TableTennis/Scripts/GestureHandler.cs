using Microsoft.MixedReality.Toolkit.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GestureHandler : MonoBehaviour, IMixedRealityGestureHandler<Vector3>
{

    [Header("Mapped gesture input actions")]

    [SerializeField]
    private MixedRealityInputAction holdAction = MixedRealityInputAction.None;

    [SerializeField]
    private MixedRealityInputAction navigationAction = MixedRealityInputAction.None;

    [SerializeField]
    private MixedRealityInputAction manipulationAction = MixedRealityInputAction.None;

    [SerializeField]
    private MixedRealityInputAction tapAction = MixedRealityInputAction.None;

    private void OnEnable()
    {
    }

    public void OnGestureStarted(InputEventData eventData)
    {
        Debug.Log($"OnGestureStarted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        MixedRealityInputAction action = eventData.MixedRealityInputAction;
        if (action == holdAction)
        {
        }
        else if (action == manipulationAction)
        {
        }
        else if (action == navigationAction)
        {
        }

    }

    public void OnGestureUpdated(InputEventData eventData)
    {
        Debug.Log($"OnGestureUpdated [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        MixedRealityInputAction action = eventData.MixedRealityInputAction;
        if (action == holdAction)
        {
        }
    }

    public void OnGestureUpdated(InputEventData<Vector3> eventData)
    {
        Debug.Log($"OnGestureUpdated [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        MixedRealityInputAction action = eventData.MixedRealityInputAction;
        if (action == manipulationAction)
        {
        }
        else if (action == navigationAction)
        {
        }
    }

    public void OnGestureCompleted(InputEventData eventData)
    {
        Debug.Log($"OnGestureCompleted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        MixedRealityInputAction action = eventData.MixedRealityInputAction;
        if (action == holdAction)
        {
        }
        else if (action == tapAction)
        {
        }
    }

    public void OnGestureCompleted(InputEventData<Vector3> eventData)
    {
        Debug.Log($"OnGestureCompleted [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        MixedRealityInputAction action = eventData.MixedRealityInputAction;
        if (action == manipulationAction)
        {
        }
        else if (action == navigationAction)
        {
        }
    }

    public void OnGestureCanceled(InputEventData eventData)
    {
        Debug.Log($"OnGestureCanceled [{Time.frameCount}]: {eventData.MixedRealityInputAction.Description}");

        MixedRealityInputAction action = eventData.MixedRealityInputAction;
        if (action == holdAction)
        {
        }
        else if (action == manipulationAction)
        {
        }
        else if (action == navigationAction)
        {
        }
    }
}
