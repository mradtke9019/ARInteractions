/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.Onnx;
using System.Reflection;
using Assets.ARInteractionData;

public class MLModel
{
    public string ONNXModelPath;
    private MLContext mlContext;
    private ITransformer onnxPredictionPipeline;
    private PredictionEngine<OnnxInput,OnnxOutput> onnxPredictionEngine;
    // Start is called before the first frame update
    public MLModel(string model)
    {
        ONNXModelPath = model;
        mlContext = new MLContext();
        List<string> inputColumns = typeof(OnnxInput).GetProperties().Select(p => p.Name).ToList();
        List<string> outputColumns = typeof(OnnxOutput).GetProperties().Select(p => p.Name).ToList();
        var pipeline =
            mlContext
            .Transforms
            .ApplyOnnxModel(
                outputColumnNames: outputColumns.ToArray(),
                inputColumnNames: inputColumns.ToArray(),
                model);
        var emptyDv = mlContext.Data.LoadFromEnumerable(new OnnxInput[] { });

        onnxPredictionPipeline = pipeline.Fit(emptyDv);
        onnxPredictionEngine = mlContext.Model.CreatePredictionEngine<OnnxInput, OnnxOutput>(onnxPredictionPipeline);
    }

    public Pose Predict(HandData data)
    {
        OnnxInput input = Newtonsoft.Json.JsonConvert.DeserializeObject<OnnxInput>(data.ToJsonString());
        var prediction = onnxPredictionEngine.Predict(input);
        return (Pose)(int)prediction.Label;
    }
}
*/