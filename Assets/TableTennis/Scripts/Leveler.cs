using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leveler : MonoBehaviour
{
    [Tooltip("List of game objects to re level in the scene.")]
    public List<GameObject> Objects;


    // Update is called once per frame
    public void Level()
    {
        foreach(var go in Objects)
        {
            go.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
