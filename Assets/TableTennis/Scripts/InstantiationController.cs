using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiationController : MonoBehaviour
{
    public GameObject Prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPrefab()
    {
        Instantiate(Prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
