using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Projectable ProjectilePath;
    private Vector3 initialLaunchDirection;
    private float launchDuration;
    private bool launching;
    // Start is called before the first frame update
    void Start()
    {
        launchDuration = 0;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (launching)
        {
            launchDuration += Time.deltaTime;
            Vector3 delta = initialLaunchDirection  * Time.deltaTime;
            this.gameObject.transform.position += delta;


            if (launchDuration >= ProjectilePath.Duration)
            {
                launching = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    public void Launch()
    {
        Stopwatch watch = Stopwatch.StartNew();
        launchDuration = 0;
        launching = true;
        this.gameObject.transform.position = ProjectilePath.GetOrigin();
        this.gameObject.SetActive(true);
        initialLaunchDirection = ProjectilePath.GetDirection().normalized;
    }

    public void Launch(Projectable projectable)
    {
        Stopwatch watch = Stopwatch.StartNew();
        launchDuration = 0;
        ProjectilePath = projectable;
        launching = true;
        this.gameObject.transform.position = ProjectilePath.GetOrigin();
        this.gameObject.SetActive(true);
        initialLaunchDirection = ProjectilePath.GetDirection().normalized;
    }
}
