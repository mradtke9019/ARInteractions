using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandData : IData
{
    private const string JSON_FORMAT = "\"{0}\": {1}";
    public Dictionary<string, float> Hand { get; set; }

    public HandData()
    {
        Hand = new Dictionary<string, float>();
    }

    public string ToJsonString()
    {
        string json = string.Empty;

        var data = Hand.Select(x => string.Format(JSON_FORMAT, x.Key, x.Value));
        json = "{" + string.Join(",", data) + "}";

        return json;
    }

    public string ToFlaskParameter()
    {
        string json = string.Empty;

        var data = Hand.Select(x => x.Value);
        data = data.Skip(1);
        json = "{\"data\":[" + string.Join(",", data) + "]}";

        return json;
    }
}
