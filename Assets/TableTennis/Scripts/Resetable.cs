using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetable : MonoBehaviour
{
    private Vector3 position;
    List<Component> _components;

    // Start is called before the first frame update
    void Start()
    {
        position = this.transform.position;
           _components = new List<Component>();
        foreach (var component in GetComponents<Component>())
        {
            if (component != this) _components.Add(component);
        }
    }

    public void ResetToInitial()
    {
        this.transform.SetPositionAndRotation(position, transform.rotation);
        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
