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
dataset = Dataset(os.path.join(cwd, "Final.json"), Debug = True)
datasetRotation = Dataset(os.path.join(cwd, "RotationFinal.json"), Debug = True)
# idealK = 51
KNNModel = MLModel()
# KNNModel.AssignModelAndHyperParameters("KNN", K = idealK)
# KNNModel.TrainModel(dataset, True)
KNNModel.LoadModel(os.path.join(cwd, "ModelKNN.joblib"), "KNN" , dataset, True)

# idealC = 10
SVCModelOriginal = MLModel()
SVCModelOriginal.LoadModel(os.path.join(cwd, "ModelSVC.joblib"), "SVC", dataset, True)


SVCModelRotation = MLModel()
SVCModelRotation.LoadModel(os.path.join(cwd, "RotationFeaturesModelSVC.joblib"), "SVC", datasetRotation, True)
print("Classes", SVCModelOriginal.model.classes_)

class Pose(Resource):
    def get(self):

        return 0, 200  # return data and 200 OK

    def post(self):
        parser = reqparse.RequestParser()  # initialize
        parser.add_argument('needScale', required=True)  # add args
        parser.add_argument('model', required=True)  # add args
        args = parser.parse_args()  # parse arguments to dictionary
        needScale = args["needScale"]
        m = args["model"]


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
        

        if m == "KNN":
            chosenModel = KNNModel
        elif m == "SVC" and featureCount == 411:
            chosenModel = SVCModelRotation
        elif m == "SVC" and featureCount == 573:
            chosenModel = SVCModelOriginal
        # probabilities = chosenModel.model.predict_proba(x)
        # print("Probs", probabilities)
        out = chosenModel.Predict(x, scale = needScale)
        # print("Change")
        # print("Prediction",out)
        return out.tolist()
                    
                    
api.add_resource(Pose, '/pose')  # add endpoints

if __name__ == '__main__':
    app.run()  # run our Flask app