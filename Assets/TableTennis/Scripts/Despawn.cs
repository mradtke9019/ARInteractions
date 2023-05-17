using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn : MonoBehaviour
{
    /// <summary>
    /// The point the object will be anchored around.
    /// </summary>
    [Tooltip("The point the object will be anchored around.")]
    public Vector3 Anchor = Vector3.zero;

    /// <summary>
    /// The distance from the anchor the object will despawn at.
    /// </summary>
    [Tooltip("The distance from the anchor the object will despawn at.")]
    public float Radius;

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(Anchor, this.transform.position);
        if(distance > Radius)
        {
            Destroy(this.gameObject);
        }
    }
}
