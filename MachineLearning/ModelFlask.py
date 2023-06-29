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

cwd = os.path.abspath(sys.path[0])
# # dataPath = "./Data/"
# # mergedDataPath = MergeJsonAndLabel(dataPath, "./")
# dataset = Dataset(os.path.join(cwd,"Final.Json"), Debug=True)

# idealK = 13
# KNNX = dataset.trainX
# KNN = MLModel()
# KNN.TrainModel("KNN", dataset.X, dataset.y, K = idealK)
model = load(os.path.join(cwd,'ModelKNN.joblib'))


class Pose(Resource):
    def get(self):
        # parser = reqparse.RequestParser()  # initialize
        # parser.add_argument('hand', required=True)  # add args
        # args = parser.parse_args()  # parse arguments to dictionary
        # print(args)
        # data = request.get_json()

        return 0, 200  # return data and 200 OK

    def post(self):
        # file  ="C:\\Users\\mradt\\source\\repos\\ARInteractions\\MachineLearning\\Data\\Fist\\2023_06_27.07_48_23_994HandData.json"
        # df = pd.read_json(file)
        # x = df.iloc[0,1:].to_numpy()
        # # firstX = x[0,:]
        # x = x.reshape(1,-1)
        # print(x)
        # input = x.reshape(1, -1)
        body = request.get_json()
        x = body["data"]
        x = np.array(x)
        # print(x)
        x = x.reshape(1,-1)
        # print("x", x)

        out = model.predict(x)
        # print("Prediction",out)
        return out[0], 200  # return data and 200 OK
                    
                    
api.add_resource(Pose, '/pose')  # add endpoints

if __name__ == '__main__':
    app.run()  # run our Flask app