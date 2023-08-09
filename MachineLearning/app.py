from flask import Flask
from flask_restful import Resource, Api, reqparse
import pandas as pd
import ast
from joblib import dump, load
import os
from flask import request
import sys
import json
import numpy as np
from PlotUtility import *
from DataUtility import *
app = Flask(__name__)
api = Api(app)

import logging
log = logging.getLogger('werkzeug')
log.disabled = True

# Ignore warnings from sklearn. https://stackoverflow.com/questions/32612180/eliminating-warnings-from-scikit-learn
def warn(*args, **kwargs):
    pass

import warnings
warnings.warn = warn

cwd = os.path.abspath(sys.path[0])
datasetPath = os.path.join(cwd, "Final.json")
datasetOptimizedPath = os.path.join(cwd, "OptimizedFinal.json")

if(os.path.exists(datasetPath) == False):
    dataPath = "./Data/"
    MergeJsonAndLabel(dataPath, "./")

if(os.path.exists(datasetOptimizedPath) == False):
    dataPath = "./OptimizedData/"
    MergeJsonAndLabel(dataPath, "./", "OptimizedFinal.json")

dataset = Dataset(datasetPath, Debug = True)
datasetOptimized = Dataset(datasetOptimizedPath, Debug = True)

oldKNN = MLModel()
oldKNN.LoadModel(os.path.join(cwd, "OLDKNNModelKNN.joblib"), "KNN" , dataset, True)

oldSVC = MLModel()
oldSVC.LoadModel(os.path.join(cwd, "OLDKNNModelKNN.joblib"), "SVC" , dataset, True)

oldLinearSVC = MLModel()
oldLinearSVC.LoadModel(os.path.join(cwd, "OldLinearSVCModelModelLinearSVCL2.joblib"), "LinearSVCL2" , dataset, True)


KNNModel = MLModel()
KNNModel.LoadModel(os.path.join(cwd, "NewKNNModelKNN.joblib"), "KNN" , datasetOptimized, True)


SVCModelOptimized = MLModel()
SVCModelOptimized.LoadModel(os.path.join(cwd, "OptimizedFeaturesModelSVC.joblib"), "SVC", datasetOptimized, True)
print("Classes", SVCModelOptimized.model.classes_)

LinearSVCModel = MLModel()
LinearSVCModel.LoadModel(os.path.join(cwd, "OptimizedFeaturesLinearModelLinearSVCL2.joblib"), "LinearSVCL2", datasetOptimized, True)

class Pose(Resource):
    def get(self):

        return 0, 200  # return data and 200 OK

    def post(self):
        parser = reqparse.RequestParser()  # initialize
        parser.add_argument('needScale', required=True)  # add args
        parser.add_argument('model', required=True)  # add args
        parser.add_argument('old', required=False)  # add args
        args = parser.parse_args()  # parse arguments to dictionary
        needScale = args["needScale"]
        m = args["model"]
        useOldModel = False
        if "old" in args:
            useOldModel = args["old"]
            # print("Using old model")

        featureCount = -1
        x = None
        body = request.get_json()
        if "data" in body:
            x = body["data"]
            x = np.array(x)
            featureCount = len(x)
            x = x.reshape(1,-1)
        elif ("right" in body) and ("left" in body):
            x = [body["left"], body["right"]]
            featureCount = len(body["left"])
            x = np.array(x)
            # x = x.reshape(1,-1)
        else:
            x = df = pd.DataFrame([body])
            featureCount = len(body)
        # print("x", x)
        chosenModel = None
        # print("FeatureCount", featureCount)
        
        # print("Feature Count", featureCount)
        if m == "KNN":
            if useOldModel:
                chosenModel = oldKNN
            else:
                chosenModel = KNNModel
        elif m == "SVC" and featureCount == 396:
            chosenModel = SVCModelOptimized
        elif m == "SVC" and featureCount == 573 and useOldModel:
            chosenModel = oldSVC
        elif m == "LinearSVCL2" and useOldModel:
            print("using Old Linear SVC")
            chosenModel = oldLinearSVC
        elif m == "LinearSVCL2" and featureCount == 396:
            chosenModel = LinearSVCModel
            
        out = chosenModel.Predict(x, scale = needScale)
        # chosenModel.PredictOptimized(x, scale = needScale)
        # print("Change")
        # print("Prediction",out)
        return out.tolist()
                    
                    
api.add_resource(Pose, '/pose')  # add endpoints

if __name__ == '__main__':
    app.run()  # run our Flask app