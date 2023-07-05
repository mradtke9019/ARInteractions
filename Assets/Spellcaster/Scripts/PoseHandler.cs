using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.XR;

public class PoseHandler 
{
    private const float GRAB_THRESHOLD = 0.3f;
    private const bool DEBUG = false;
    private IMixedRealityHandJointService handJointService;
    private MLModel model;
    string modelName;


    public PoseHandler(MLModel model)
    {
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        string name = Enum.GetName(typeof(MLModel), model);
        this.model = model;
        modelName = name;


        Debug.Log("Using model " + name);
    }

    /// <summary>
    /// Returns what the current pose is for the given hand.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    public Pose GetPose(Handedness hand)
    {
        if (!handJointService.IsHandTracked(hand))
            return Pose.None;

        HandData handData = DataCollectionManager.GetHandData(hand, handJointService);
        return PredictPose(handData);
    }

    /// <summary>
    /// Using a pre trained machine learning model, predict from the hand data what kind of pose is being executed by the hand.
    /// </summary>
    /// <param name="handData"></param>
    /// <returns></returns>
    public Pose PredictPose(IData handData)
    {
        string address = null;
        string localAddress = "127.0.0.1";
        string remoteAddress = "192.168.0.101";
        address = Application.isEditor ? localAddress : remoteAddress;
        
        string url = $"http://{address}:5000/pose?needScale=true&model={modelName}";
        // Convert the object to JSON
        string jsonBody = handData.ToFlaskParameter();
        Pose result = Pose.None;

        using (HttpClient httpClient = new HttpClient())
        {
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

            string responseContent = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                // Request successful
/*                string r = responseContent.Trim();
                float f = float.Parse(r);*/
                result = (Pose)System.Enum.Parse(typeof(Pose), responseContent.Replace('\"',' ').Trim());
            }
            else
            {
                // Request failed
                Console.WriteLine("Error: " + responseContent);
            }
        }
        return result;
    }

    private bool Similar(float value, float center, float threshold = 0.15f)
    {
        float min = center - threshold;
        float max = center + threshold;

        return value > min && value < max;
    }
}
