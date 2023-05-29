using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    private List<Mesh> _sceneMeshes;
    // Start is called before the first frame update
    void Start()
    {
        string name = LayerMask.LayerToName(31);
        LayerMask mask = LayerMask.GetMask(new string[] { name });
        
        //_sceneMeshes = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
