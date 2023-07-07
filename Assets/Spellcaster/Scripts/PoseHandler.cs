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
    string address = null;
    string localAddress = "127.0.0.1";
    string remoteAddress = "192.168.0.101";
    string url;

    public PoseHandler(MLModel model)
    {
        handJointService = CoreServices.GetInputSystemDataProvider<IMixedRealityHandJointService>();
        string name = Enum.GetName(typeof(MLModel), model);
        this.model = model;
        modelName = name;

        address = Application.isEditor ? localAddress : remoteAddress;
        url = $"http://{address}:5000/pose?needScale=true&model={modelName}";

        Debug.Log("Using model " + name);
    }

    /// <summary>
    /// Returns what the current pose for both hands specifically with 2 entries. Entry 0 is the left hand and entry 1 is the right hand.
    /// </summary>
    /// <param name="hand"></param>
    /// <returns></returns>
    public List<Pose> GetPoses()
    {
        Handedness right = Handedness.Right;
        Handedness left = Handedness.Left;
        // If both hands are not visible, return nones.
        bool rightTracked = handJointService.IsHandTracked(right);
        bool leftTracked = handJointService.IsHandTracked(left);

        // If both are not tracked, just return none.
        if(!rightTracked && !leftTracked)
        {
            return new List<Pose>() { Pose.None, Pose.None };
        }

        List<Pose> result = new List<Pose>();

        HandData leftHandData = null;
        HandData rightHandData = null;
        // Send one or both datas to be predicted on
        if (leftTracked && !rightTracked)
        {
            leftHandData = DataCollectionManager.GetHandData(left, handJointService);
            return new List<Pose>() { PredictPose(leftHandData), Pose.None };
        }
        else if(!leftTracked && rightTracked)
        {
            rightHandData = DataCollectionManager.GetHandData(right, handJointService);
            return new List<Pose>() { Pose.None, PredictPose(rightHandData) };
        }
        else
        {
            leftHandData = DataCollectionManager.GetHandData(left, handJointService);
            rightHandData = DataCollectionManager.GetHandData(right, handJointService);
            return PredictPoses(leftHandData, rightHandData);
        }
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
    public List<Pose> PredictPoses(IData leftHandData, IData rightHandData)
    {
        // Convert the object to JSON
        string jsonBody = leftHandData.ToFlaskParameter("left", false) + "," + rightHandData.ToFlaskParameter("right", false);
        jsonBody = "{" + jsonBody + "}";

        List<Pose> result = null;

        using (HttpClient httpClient = new HttpClient())
        {
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

            string responseContent = response.Content.ReadAsStringAsync().Result;

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    List<string> responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(responseContent.Trim());
                    result = responseData.Select(x => (Pose)System.Enum.Parse(typeof(Pose), x)).ToList();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
            else
            {
                // Request failed
                Console.WriteLine("Error: " + responseContent);
            }
        }
        return result;
    }

    /// <summary>
    /// Using a pre trained machine learning model, predict from the hand data what kind of pose is being executed by the hand.
    /// </summary>
    /// <param name="handData"></param>
    /// <returns></returns>
    public Pose PredictPose(IData handData)
    {
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
                try
                {
                    List<string> responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(responseContent.Trim());
                    result = responseData.Select(x => (Pose)System.Enum.Parse(typeof(Pose), x)).First();
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
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
